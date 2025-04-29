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

        // services.AddAuthorization(cfg =>
        //     {
        //         cfg.AddPolicy("Admin", 
        //             policy => policy.RequireClaim("role", "Admin"));
        //         cfg.AddPolicy("User", 
        //             policy => policy.RequireClaim("role", "User", "Admin"));
        //         cfg.AddPolicy("ClearanceLevel1", 
        //             policy => policy.RequireClaim("ClearanceLevel", "1", "2"));
        //         cfg.AddPolicy("ClearanceLevel2", 
        //             policy => policy.RequireClaim("ClearanceLevel", "2"));
        //     });
                services.AddAuthorization(cfg =>
        {
        cfg.AddPolicy("Admin", policy => policy.RequireClaim("type", "Admin")); // שינוי: "role" במקום "type"
        cfg.AddPolicy("User", policy => policy.RequireClaim("type", "User", "Admin")); // שינוי: "role" במקום "type"
        });
        return services;
    //     services.AddAuthorization(options =>
    //    {
    //        options.AddPolicy("Admin", policy =>
    //        {
    //            policy.RequireClaim("type", "Admin");
    //        });

    //        return services;

    //    });
    }
}
