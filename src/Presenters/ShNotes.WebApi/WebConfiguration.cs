using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShNotes.WebApi.Jwt;

namespace ShNotes.WebApi;

public static class WebConfiguration
{
    public static object JwtBearerDefaults { get; private set; }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(o =>
        {
            o.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ShNotes API Documentation",
                    Description = "Документация для API ShNotes.",
                    Contact = new OpenApiContact
                    {
                        Name = "Github разработчика",
                        Url = new Uri("https://github.com/sh0rtener"),
                    },
                }
            );

            o.AddSecurityRequirement(
                new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Jwt",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        Array.Empty<string>()
                    },
                }
            );

            var basePath = AppContext.BaseDirectory;
            var xmlPath = Path.Combine(basePath, "ShNotes.WebApi.xml");
            o.IncludeXmlComments(xmlPath);
        });

        return services;
    }

    public static IServiceCollection RegisterJwt(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddOptions();
        services.Configure<JwtConfiguration>(configuration.GetSection("Jwt"));

        return services;
    }

    public static IServiceCollection AddJwtAuth(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = "Bearer";
                x.DefaultChallengeScheme = "Bearer";
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)
                    ),
                };
            });

        return services;
    }
}
