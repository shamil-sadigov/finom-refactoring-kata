using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReportService.Application;
using ReportService.Application.Report;
using ReportService.Application.Report.Abstractions;
using ReportService.Application.Resolvers.BuhCodeResolver;
using ReportService.Application.Resolvers.SalaryResolver;
using ReportService.Infrastructure;

namespace ReportService.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            
            services.AddSingleton<EmployeeModelTransformation>();
            services.AddSingleton<IReportWriter, ReportWriter>();
            services.AddSingleton<IReportInfoProvider, ReportInfoProvider>();
            
            services.AddScoped<IReportProvider, ReportProvider>();
            
            var employeeSalaryServiceUri = Configuration.GetValue<string>("EmployeeSalaryServiceUri").ThrowIfNull();
            var employeeBuhCodeServiceUri = Configuration.GetValue<string>("EmployeeBuhCodeServiceUri").ThrowIfNull();
            
            services.AddHttpClient<IEmployeeSalaryResolver, EmployeeSalaryResolver>(
                client => client.BaseAddress = new Uri(employeeSalaryServiceUri, UriKind.Absolute));
            
            services.AddHttpClient<IEmployeeBuhCodeResolver, EmployeeBuhCodeResolver>(
                client => client.BaseAddress = new Uri(employeeBuhCodeServiceUri, UriKind.Absolute));
            
            // Infra

            var connectionString = Configuration.GetConnectionString("PostgreSql").ThrowIfNull();

            services.AddScoped<IDbConnectionFactory, NgSqlConnectionFactory>(
                _ => new NgSqlConnectionFactory(connectionString));

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
