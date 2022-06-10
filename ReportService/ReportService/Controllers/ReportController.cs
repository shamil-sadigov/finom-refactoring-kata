using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Download(int year, int month)
        {
            // TODO: Заменить синхронные вызовы на асинхронный везде где возможно
            
            var actions = new List<(Action<Employee, Report>, Employee)>();
            var report = new Report() { S = MonthNameResolver.MonthName.GetName(year, month) };
            
            // TODO: Вынести в appsettings
            const string connString = "Host=192.168.99.100;Username=postgres;Password=1;Database=employee";
            
            // TODO: Стоит абстрагироваться от NG + зарегистрировать в DI контейнере и позаботиться о Dispose
            var conn = new NpgsqlConnection(connString);
            
            conn.Open();
            
            // TODO: Зачем нужно два отдельных запроса cmd и cmd1 ? Стоит обьединить в один запрос
            
            var cmd = new NpgsqlCommand("SELECT d.name from deps d where d.active = true", conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                List<Employee> emplist = new List<Employee>();
                var depName = reader.GetString(0);
                var conn1 = new NpgsqlConnection(connString);
                conn1.Open();
                var cmd1 = new NpgsqlCommand("SELECT e.name, e.inn, d.name from emps e left join deps d on e.departmentid = d.id", conn1);
                var reader1 = cmd1.ExecuteReader();
                while (reader1.Read())
                {
                    var emp = new Employee()
                    {
                        Name = reader1.GetString(0), 
                        Inn = reader1.GetString(1), 
                        Department = reader1.GetString(2)
                    };
                    // TODO: Не нужно блокировать поток
                    // TODO: Стоит абстрагироваться от EmpCodeResolver ради decreased coupling + тестирование
                    emp.BuhCode = EmpCodeResolver.GetCode(emp.Inn).Result;
                    emp.Salary = await _salaryProvider.GetSalaryAsync(emp, CancellationToken.None); 
                    
                    // TODO: Давайте не будем так делать
                    if (emp.Department != depName)
                        continue;
                    emplist.Add(emp);
                }

                actions.Add((new ReportFormatter(null).NL, new Employee()));
                actions.Add((new ReportFormatter(null).WL, new Employee()));
                actions.Add((new ReportFormatter(null).NL, new Employee()));
                actions.Add((new ReportFormatter(null).WD, new Employee() { Department = depName } ));
                
                for (int i = 1; i < emplist.Count(); i ++)
                {
                    actions.Add((new ReportFormatter(emplist[i]).NL, emplist[i]));
                    actions.Add((new ReportFormatter(emplist[i]).WE, emplist[i]));
                    actions.Add((new ReportFormatter(emplist[i]).WT, emplist[i]));
                    actions.Add((new ReportFormatter(emplist[i]).WS, emplist[i]));
                }  

            }
            actions.Add((new ReportFormatter(null).NL, null));
            actions.Add((new ReportFormatter(null).WL, null));

            foreach (var act in actions)
            {
                act.Item1(act.Item2, report);
            }
            report.Save();
            
            var file = System.IO.File.ReadAllBytes("D:\\report.txt");
            var response = File(file, "application/octet-stream", "report.txt");
            return response;
        }
    }
}
