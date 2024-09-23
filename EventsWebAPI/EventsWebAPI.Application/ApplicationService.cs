using AutoMapper;
using EventsWebAPI.Application.MapperProfile;
using EventsWebAPI.Application.Jwt.JwtDataProviderService;
using EventsWebAPI.Application.Jwt.JwtTokenProviderService;
using EventsWebAPI.Application.Services;
using EventsWebAPI.Application.Validation.Events;
using EventsWebAPI.Application.Validation.Users;
using EventsWebAPI.Application.Dto_s.Requests.Event;
using EventsWebAPI.Application.Dto_s.Requests.User;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;


namespace EventsWebAPI.Application
{
    public static class ApplicationService
    {
        public static IServiceCollection AddAppService(this IServiceCollection services)
        {

            services.AddScoped<EventService>();
            services.AddScoped<UserService>();
            services.AddScoped<AttendingService>();

            services.AddScoped<IValidator<CreateEventRequest>, CreateEventModelValidator>();
            services.AddScoped<IValidator<UpdateEventRequest>, UpdateEventModelValidator>();
            services.AddScoped<IValidator<RegisterUserRequest>, CreateUserModelValidator>();

            services.AddScoped<JwtTokenProviderService>();
            services.AddScoped<JwtDataProviderService>();

            services.AddAutoMapper(typeof(EventMappingProfile), typeof(UserMappingProfile));

            return services;
        }
    }
}
