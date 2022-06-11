using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using ReportService.Domain;
using ReportService.Services;
using ReportService.Services.BuhCodeResolver;
using ReportService.Services.SalaryProvider;

namespace ReportService.Controllers
{
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        private readonly IEmployeeSalaryProvider _salaryProvider;
        private readonly IEmployeeCodeResolver _employeeCodeResolver;

        public ReportController(IEmployeeSalaryProvider salaryProvider, IEmployeeCodeResolver employeeCodeResolver)
        {
            _salaryProvider = salaryProvider;
            _employeeCodeResolver = employeeCodeResolver;
        }
        
        // TODO: Стоит вынести логику логику построения отчетов в отдельный модуль
        // TODO: Почему бы нам не кешировать отчеты которые мы уже генерили ранее ? Хмм ? Хммм ?
        // TODO: Не нужно ли подумать о локализации отчетов ?
        // TODO: It's better to use different models for building reports and quering DB in order to reduce coupling
        // TODO: Add exception handling
        [HttpGet]
        [Route("{year}/{month}")]
        public async Task<IActionResult> Download(int year, int month, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            // TODO: Вынести в appsettings
            const string connString = "Host=192.168.99.100;Username=postgres;Password=1;Database=employee";
            
            // TODO: Стоит абстрагироваться от NG + зарегистрировать в DI контейнере и позаботиться о Dispose
            
            var sqlConnection = new NpgsqlConnection(connString);
            
            await sqlConnection.OpenAsync(cancellationToken);
            
            // TODO: Check how dapper mapping behaves in case of missing marching columns
            
            IReadOnlyList<Employee> employees = await GetEmployeesFromDbAsync(sqlConnection);

            await ResolveEmployeeSalariesAsync(employees, cancellationToken);

            var departmentReportItems = employees.GroupBy(x => x.Department)
                .Select(x =>
                {
                    var employeesReportItems = x.Select(y => new EmployeeReportItem(y.Name, y.Salary)).ToArray();
                    return new DepartmentReportItem(DepartmentName: x.Key, employeesReportItems);
                });
            
            // report.Save();
            
            // Return stream instead of reading bytes
            byte[] file = await System.IO.File.ReadAllBytesAsync("D:\\report.txt", cancellationToken);
            var response = File(file, "application/octet-stream", "report.txt");

            return response;
        }

        private async Task ResolveEmployeeSalariesAsync(
            IReadOnlyList<Employee> employees,
            CancellationToken cancellationToken)
        {
            List<Task> employeeSalaryResolverTasks = new();

            foreach (var employee in employees)
            {
                // TODO: Стоит абстрагироваться от EmpCodeResolver ради decreased coupling + тестирование

                var salaryResolverTask = _employeeCodeResolver
                    .GetEmployeeBuhcodeAsync(employee.Inn, cancellationToken)
                    .ContinueWith(async codeResolverTask =>
                    {
                        if (!codeResolverTask.IsCompletedSuccessfully)
                        {
                            throw new InvalidOperationException(
                                "Something went wrong during getting employee Inn",
                                codeResolverTask.Exception);
                        }

                        var employeeBuhCode = codeResolverTask.Result;

                        employee.Salary = 
                            await _salaryProvider.GetSalaryAsync(employeeBuhCode, employee.Inn, cancellationToken);
                    }, cancellationToken).Unwrap();

                employeeSalaryResolverTasks.Add(salaryResolverTask);
            }

            await Task.WhenAll(employeeSalaryResolverTasks);
        }

        private static async Task<IReadOnlyList<Employee>> GetEmployeesFromDbAsync(NpgsqlConnection sqlConnection)
        {
            var employees =  await sqlConnection.QueryAsync<Employee>(
                @"SELECT employees.name AS Name, 
                         employees.inn AS Inn, 
                         departments.name AS Department,
                  FROM employees
                  LEFT JOIN departments ON employees.departmentid = departments.id
                  WHERE departments.active = true");

            return employees.ToList();
        }
    }
}
