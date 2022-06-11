using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using ReportService.Domain;
using ReportService.Services;
using ReportService.Services.BuhCodeResolver;
using ReportService.Services.Report;
using ReportService.Services.SalaryProvider;

/*
 *  NOTES:
 *
 * - Предполагаю что отчеты созданные за прошлые месяцы уже имутабельны а значит их можно сохранять
 * и при вовтороном запросе возвращать уже готовые сораненные отчеты а не генерить заново.
 * В рамках текущего решения отчеты сохраняются в локальной файловой системе, но конечно в продашне
 * этого делать не стоит, потому docker контейнеры как правило без состояния, а нам нужно более надежное хранилище.
 * Какие варианты ?
 *
 * - Если у нас кубер, то можно сохраняться в Persistent Volume Storage
 * - Или же можно сохранять хранилище S3.
 * - Или другие варианты.
 *
 *
 * - Я бы не стал делать синронные вызовы в сторонние сервисы чтобы получить BuhCode и Salary,
 * это не по микросервистски потому что может привести к reduced availability.
 * Я бы попробовал подписаться на нужные мне события и держать локальную реплику нужных мне данных,
 * таким образом при необходимости я могу брать данные из реплики а не запрашивать у сервисов
 * а актуальность данных поддерживать через события
 * (но и тут нужно подумать, потому что это уже eventual consistency,
 * следует убедиться что мы не столкнемся с негативными последвтсиями временно несогласованных данных).
 *
 * 
 * Я бы добавил локализацию, но думаю это не сложно и не является целью нашей задачи
 *
 * Можно было бы добавить отказоустойчивости при запросе внешних сервисом, но мне кажется это не цель задачи
 *
 * Можно было бы добавит логгирования
 */


namespace ReportService.Controllers
{
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        private readonly IEmployeeSalaryProvider _salaryProvider;
        private readonly IEmployeeCodeResolver _employeeCodeResolver;
        private readonly EmployeeModelTransformation _employeeModelTransformation;
        private readonly IReportService _reportService;

        public ReportController(
            IEmployeeSalaryProvider salaryProvider, 
            IEmployeeCodeResolver employeeCodeResolver,
            EmployeeModelTransformation employeeModelTransformation,
            IReportService reportService)
        {
            _salaryProvider = salaryProvider;
            _employeeCodeResolver = employeeCodeResolver;
            _employeeModelTransformation = employeeModelTransformation;
            _reportService = reportService;
        }
        
        // TODO: Стоит вынести логику логику построения отчетов в отдельный модуль
        // TODO: Почему бы нам не кешировать отчеты которые мы уже генерили ранее ? Хмм ? Хммм ?
        // TODO: Не нужно ли подумать о локализации отчетов ?
        // TODO: It's better to use different models for building reports and quering DB in order to reduce coupling
        // TODO: Add exception handling
        [HttpGet("{year}/{month}")]
        public async Task<IActionResult> Download(int year, int month, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            // TODO: Вынести в appsettings
            const string connString = "Host=192.168.99.100;Username=postgres;Password=1;Database=employee";
            
            // TODO: Стоит абстрагироваться от NG + зарегистрировать в DI контейнере и позаботиться о Dispose
            
            var sqlConnection = new NpgsqlConnection(connString);
            
            await sqlConnection.OpenAsync(cancellationToken);
            
            // TODO: Check how dapper mapping behaves in case of missing marching columns

            IReadOnlyList<EmployeeModel> employees = await GetEmployeesFromDbAsync(sqlConnection);

            EmployeeReportItem[] employeeReportItems = 
                await _employeeModelTransformation.TransformToReportableItemsAsync(employees, cancellationToken);
            
            var reportLocation = 
                await _reportService.CreateReportAsync(new AccountingReportParams(year, month, employeeReportItems));
            
            // report.Save();
            
            // Return stream instead of reading bytes
            byte[] file = await System.IO.File.ReadAllBytesAsync("D:\\report.txt", cancellationToken);
            var response = File(file, "application/octet-stream", "report.txt");
            
            return response;
        }


        private static async Task<IReadOnlyList<EmployeeModel>> GetEmployeesFromDbAsync(IDbConnection sqlConnection)
        {
            var employees =  await sqlConnection.QueryAsync<EmployeeModel>(
                @"SELECT employees.name AS Name, 
                         employees.inn AS Inn, 
                         departments.name AS Department
                  FROM employees
                  LEFT JOIN departments ON employees.departmentid = departments.id
                  WHERE departments.active = true");

            return employees.ToList();
        }
    }
}
