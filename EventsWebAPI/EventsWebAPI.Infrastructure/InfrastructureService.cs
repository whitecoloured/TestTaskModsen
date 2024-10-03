using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using EventsWebAPI.Context;
using EventsWebAPI.Infrastructure.Repositories.Interfaces;
using EventsWebAPI.Infrastructure.Repositories.Implementations;

namespace EventsWebAPI.Infrastructure
{
    public static class InfrastructureService
    {

        public static IServiceCollection AddRepoService(this IServiceCollection services)
        {
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAttendingRepository, AttendingRepository>();

            return services;
        }
    }
}
