using MassTransit;
using MessageBroker;
using Microsoft.EntityFrameworkCore;
using SagaStateMachineWorkerService;
using SagaStateMachineWorkerService.Models;
using System.Reflection;

internal class Program
{
    private static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .Build();

        var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMassTransit(conf =>
        {
            conf.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>().EntityFrameworkRepository(opt =>
            {//state machine'nýn sql server kullanýlacaðý belirtildi.
                opt.AddDbContext<DbContext, OrderStateDbContext>((provider, builder) =>
                {
                    builder.UseSqlServer(configuration.GetConnectionString("SqlCon"), m =>
                    {
                        m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                    });
                });
            });
            conf.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(configure =>
            {
                configure.Host(configuration.GetConnectionString("RabbitMQ"));

                configure.ReceiveEndpoint(RabbitMQSettingsConst.OrderSaga, e =>
                {
                    e.ConfigureSaga<OrderStateInstance>(provider);
                });
            }));
        });

        services.AddMassTransitHostedService();
        services.AddHostedService<Worker>();
    })
    .Build();

        host.Run();
    }
}