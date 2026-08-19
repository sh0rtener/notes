using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace ShNotes.UseCases;

public static class DependencyInjection
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddAutoMapper(c =>
        {
            c.AddMaps(Assembly.GetExecutingAssembly());
        });
        services.AddMediatR(c =>
            c.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
        );
        services.AddSwaggerGen(o =>
        {
            var basePath = AppContext.BaseDirectory;
            var xmlPath = Path.Combine(basePath, "ShNotes.UseCases.xml");
            o.IncludeXmlComments(xmlPath);
        });

        return services;
    }
}
