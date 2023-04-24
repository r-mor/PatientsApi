using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Patients.Application.Database;
using Patients.Application.DataProtector;
using Patients.Application.Repository;
using Patients.Application.Services;

namespace Patients.Application.ServiceCollectionExtentions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IPatientRepository, PatientRepositorySqlServerExpress>();
        services.AddSingleton<IPatientService, PatientService>();
        services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ =>
        new SqlServerExpressConnectionFactory(connectionString));

        services.AddSingleton<DbInitializer>();
        return services;
    }

    public static IServiceCollection AddDataProtector(this IServiceCollection services, string encryptionKey)
    {
        services.AddSingleton(_ => new DataEncryption(encryptionKey));
        services.AddSingleton<IPatientDataProtector, PatientDataProtector>();

        return services;
    }



}
