using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using EventsWebAPI.Context;
using EventsWebAPI.Infrastructure.Repositories.Interfaces;
using EventsWebAPI.Infrastructure.Repositories.Implementations;

namespace EventsWebAPI.Infrastructure
{
    public static class InfrastructureService
    {
        public static IServiceCollection AddEFService(this IServiceCollection service)
        {
            service.AddEntityFrameworkSqlServer().AddDbContext<EventDbContext>(options =>
                options.UseSqlServer("Data Source=WIN-Q28J0UDQIV6\\SQLEXPRESS;Initial Catalog = eventsDB;Integrated Security = True;"));

            return service;
        }

        public static IServiceCollection AddRepoService(this IServiceCollection services)
        {
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAttendingRepository, AttendingRepository>();

            return services;
        }
    }
}
