using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using ReportService.Domain;
using ReportService.Services;

namespace ReportService.Controllers
{
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        private readonly IEmployeeSalaryProvider _salaryProvider;

        public ReportController(IEmployeeSalaryProvider salaryProvider)
        {
            _salaryProvider = salaryProvider;
        }
        
        
        // TODO: Стоит вынести логику логику построения отчетов в отдельный модуль
        // TODO: Почему бы нам не кешировать отчеты которые мы уже генерили ранее ? Хмм ? Хммм ?
        // TODO: Не нужно ли подумать о локализации отчетов ?
        [HttpGet]
        [Route("{year}/{month}")]
        public async Task<IActionResult> Download(int year, int month, CancellationToken cancellationToken)
        {
            // TODO: Заменить синхронные вызовы на асинхронный везде где возможно
            
            // var actions = new List<(Action<Employee, Report>, Employee)>();
            var report = new Report() { Content = MonthNameResolver.MonthName.GetName(year, month) };
            
            // TODO: Вынести в appsettings
            const string connString = "Host=192.168.99.100;Username=postgres;Password=1;Database=employee";
            
            // TODO: Стоит абстрагироваться от NG + зарегистрировать в DI контейнере и позаботиться о Dispose
            
            var sqlConnection = new NpgsqlConnection(connString);
            
            await sqlConnection.OpenAsync(cancellationToken);
            
            // TODO: Check how dapper mapping behaves in case of missing marching columns
            
            List<Employee> employees = await GetEmployeesFromDbAsync(sqlConnection);
            
            foreach (var employee in employees)
            {
                // TODO: Не нужно блокировать поток
                // TODO: Стоит абстрагироваться от EmpCodeResolver ради decreased coupling + тестирование
                employee.BuhCode = await EmpCodeResolver.GetCodeAsync(employee.Inn);
                employee.Salary = await _salaryProvider.GetSalaryAsync(employee, CancellationToken.None); 
            }
            
            // actions.Add((new ReportFormatter(null).NL, new Employee()));
            // actions.Add((new ReportFormatter(null).WL, new Employee()));
            // actions.Add((new ReportFormatter(null).NL, new Employee()));
            // actions.Add((new ReportFormatter(null).WD, new Employee() { Department = depName } ));
            
            for (int i = 1; i < employees.Count(); i ++)
            {
                // actions.Add((new ReportFormatter(emplist[i]).NL, emplist[i]));
                // actions.Add((new ReportFormatter(emplist[i]).WE, emplist[i]));
                // actions.Add((new ReportFormatter(emplist[i]).WT, emplist[i]));
                // actions.Add((new ReportFormatter(emplist[i]).WS, emplist[i]));
            }  
                
            // actions.Add((new ReportFormatter(null).NL, null));
            // actions.Add((new ReportFormatter(null).WL, null));

            // foreach (var act in actions)
            // {
            //     act.Item1(act.Item2, report);
            // }
            //
            report.Save();
            
            var file = await System.IO.File.ReadAllBytesAsync("D:\\report.txt", cancellationToken);
            var response = File(file, "application/octet-stream", "report.txt");
            return response;
        }

        private static async Task<List<Employee>> GetEmployeesFromDbAsync(NpgsqlConnection sqlConnection)
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
