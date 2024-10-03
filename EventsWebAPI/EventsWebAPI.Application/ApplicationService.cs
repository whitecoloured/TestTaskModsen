using AutoMapper;
using EventsWebAPI.Application.MapperProfile;
using EventsWebAPI.Application.Jwt.JwtDataProviderService;
using EventsWebAPI.Application.Jwt.JwtTokenProviderService;
using EventsWebAPI.Application.Validation.Events;
using EventsWebAPI.Application.Validation.Users;
using EventsWebAPI.Application.Commands_and_Queries.Events.CreateEvent;
using EventsWebAPI.Application.Commands_and_Queries.Events.UpdateEvent;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System.Reflection;
using EventsWebAPI.Application.Commands_and_Queries.Users.Register;

namespace EventsWebAPI.Application
{
    public static class ApplicationService
    {
        public static IServiceCollection AddAppService(this IServiceCollection services)
        {

            services.AddScoped<IValidator<CreateEventCommand>, CreateEventModelValidator>();
            services.AddScoped<IValidator<UpdateEventRequest>, UpdateEventModelValidator>();
            services.AddScoped<IValidator<RegisterUserCommand>, CreateUserModelValidator>();

            services.AddScoped<JwtTokenProviderService>();
            services.AddScoped<JwtDataProviderService>();

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddAutoMapper(typeof(EventMappingProfile), typeof(UserMappingProfile));

            return services;
        }
    }
}
