using Microsoft.EntityFrameworkCore;
using OrderAPI.Repository;

namespace OrderAPI.Resolver
{
    public static class ServiceResolver
    {
        public static IConfiguration Configuration { get; set; }


        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("Sql"));
            });
        }
    }
}