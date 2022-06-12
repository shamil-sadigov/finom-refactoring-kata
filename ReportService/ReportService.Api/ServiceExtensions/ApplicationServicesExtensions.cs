using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReportService.Application;
using ReportService.Application.Report;
using ReportService.Application.Report.Abstractions;
using ReportService.Application.Resolvers.BuhCodeResolver;
using ReportService.Application.Resolvers.SalaryResolver;

namespace ReportService.Api.ServiceExtensions;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        var employeeSalaryServiceUri = configuration.GetValue<string>("EmployeeSalaryServiceUri").ThrowIfNull();
        var employeeBuhCodeServiceUri = configuration.GetValue<string>("EmployeeBuhCodeServiceUri").ThrowIfNull();
        var reportDirectoryRoot = GetDirectoryPreparedForReports(configuration);

        services.AddHttpClient<IEmployeeSalaryResolver, EmployeeSalaryResolver>(
            client => client.BaseAddress = new Uri(employeeSalaryServiceUri, UriKind.Absolute));
            
        services.AddHttpClient<IEmployeeBuhCodeResolver, EmployeeBuhCodeResolver>(
            client => client.BaseAddress = new Uri(employeeBuhCodeServiceUri, UriKind.Absolute));
        
         services.AddScoped<EmployeeTransformation>()
            .AddSingleton<IReportWriter, ReportWriter>()
            .AddSingleton<IReportInfoProvider, ReportInfoProvider>(_ => new ReportInfoProvider(reportDirectoryRoot))
            .AddScoped<IReportProvider, ReportProvider>();
        
        return services;
    }

    private static string GetDirectoryPreparedForReports(IConfiguration configuration)
    {
        var reportsDirectoryName = configuration.GetValue<string>("ReportsUploadDirectoryName").ThrowIfNull();
        var reportDirectoryRoot = Path.Combine(Directory.GetCurrentDirectory(), reportsDirectoryName);
        Directory.CreateDirectory(reportDirectoryRoot);
        return reportDirectoryRoot;
    }
}