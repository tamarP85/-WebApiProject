using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services;

namespace Services;

public static class AuthServiceExtensions
{
    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.TokenValidationParameters =
                    TokenService.GetTokenValidationParameters();
            });

       
                services.AddAuthorization(cfg =>
        {
        cfg.AddPolicy("Admin", policy => policy.RequireClaim("type", "Admin")); // שינוי: "role" במקום "type"
        cfg.AddPolicy("Agent", policy => policy.RequireClaim("type", "Agent", "Admin")); // שינוי: "role" במקום "type"
        });
        return services;
    }
}
