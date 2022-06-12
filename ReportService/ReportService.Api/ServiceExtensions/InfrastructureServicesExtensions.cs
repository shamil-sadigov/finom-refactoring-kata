using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReportService.Application;
using ReportService.Infrastructure;

namespace ReportService.Api.ServiceExtensions;

public static class InfrastructureServicesExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgreSql").ThrowIfNull();

        services.AddScoped<IDbConnectionFactory, NgSqlConnectionFactory>(
            _ => new NgSqlConnectionFactory(connectionString));

        services.AddScoped<IEmployeeRepository, EmployeeRepository>();

        return services;
    }
}