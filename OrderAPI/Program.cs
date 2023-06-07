using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Repository;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, conf) =>
    {
        conf.Host(builder.Configuration.GetConnectionString("RabbitMQ"));
    });
});
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Sql"));
});

builder.Services.AddMassTransitHostedService();     
// Add services to the container
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
