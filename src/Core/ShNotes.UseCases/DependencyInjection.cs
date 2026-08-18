using Microsoft.Extensions.DependencyInjection;

namespace ShNotes.UseCases;

public static class DependencyInjection
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddAutoMapper(c => AppDomain.CurrentDomain.GetAssemblies());
        services.AddMediatR(c =>
            c.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
        );

        return services;
    }
}
