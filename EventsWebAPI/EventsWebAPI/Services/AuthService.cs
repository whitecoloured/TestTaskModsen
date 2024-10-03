using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using EventsWebAPI.Core.Enums;
using EventsWebAPI.Core.Options;

namespace EventsWebAPI.Services
{

    public static class AuthService
    {

        public static IServiceCollection AddAuthService(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SecretKey))
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy =>
                {
                    policy.RequireClaim("Role", UserRole.Admin.ToString());
                });

                options.AddPolicy("UserPolicy", policy =>
                {
                    policy.RequireClaim("ID");
                });
            }
            );



            return services;
        }
    }
}
