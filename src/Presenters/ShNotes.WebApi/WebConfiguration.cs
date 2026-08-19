using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace ShNotes.WebApi;

public static class WebConfiguration
{
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

            var basePath = AppContext.BaseDirectory;
            var xmlPath = Path.Combine(basePath, "ShNotes.WebApi.xml");
            o.IncludeXmlComments(xmlPath);
        });

        return services;
    }
}
