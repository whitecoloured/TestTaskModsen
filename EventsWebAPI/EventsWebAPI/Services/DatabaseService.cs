using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using EventsWebAPI.Context;
using Microsoft.Extensions.Configuration;

namespace EventsWebAPI.Services
{
    public static class DatabaseService
    {
        public static IServiceCollection AddDatabase(this IServiceCollection service, IConfiguration config)
        {
            service.AddDbContext<EventDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("Database"), b=> b.MigrationsAssembly("EventsWebAPI.Infrastructure")));

            return service;
        }
    }
}
