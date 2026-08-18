using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShNotes.Data.EntityFramework;
using ShNotes.UseCases.Notes;

namespace ShNotes.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddData(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddAutoMapper(c => Assembly.GetExecutingAssembly());
        services.AddScoped<IScriptProvider, SqliteScriptProvider>();
        services.AddEf(configuration);
        services.AddScoped<INoteRepository, UseCaseNoteRepository>();

        return services;
    }

    public static IServiceCollection AddEf(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionType =
            configuration.GetSection("dbtype").Value
            ?? throw new InvalidDataException("ConnectionType ('DbType') is empty");

        switch (connectionType)
        {
            case ConnectionTypes.MsSql:
            {
                var connectionString =
                    configuration.GetConnectionString(ConnectionTypes.MsSql)
                    ?? throw new InvalidDataException("Connection string is empty");

                // TODO: add connection
                break;
            }

            case ConnectionTypes.PgSql:
            {
                var connectionString =
                    configuration.GetConnectionString(ConnectionTypes.PgSql)
                    ?? throw new InvalidDataException("Connection string is empty");

                // TODO: add connection
                break;
            }

            case ConnectionTypes.Sqlite:
            {
                var connectionString =
                    configuration.GetConnectionString(ConnectionTypes.Sqlite)
                    ?? throw new InvalidDataException("Connection string is empty");

                services.AddDbContext<AppDbContext>(x => x.UseSqlite(connectionString));
                break;
            }
        }

        return services;
    }
}
