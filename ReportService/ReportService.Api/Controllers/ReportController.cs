using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReportService.Application;
using ReportService.Application.BuhCodeResolver;
using ReportService.Application.Report;
using ReportService.Application.Report.Abstractions;
using ReportService.Application.SalaryProvider;

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

namespace ReportService.Api.Controllers
{
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        private readonly ReportProvider _reportProvider;

        public ReportController(ReportProvider reportProvider)
        {
            _reportProvider = reportProvider;
        }
        
        // TODO: Стоит вынести логику логику построения отчетов в отдельный модуль
        // TODO: Почему бы нам не кешировать отчеты которые мы уже генерили ранее ? Хмм ? Хммм ?
        // TODO: Не нужно ли подумать о локализации отчетов ?
        // TODO: It's better to use different models for building reports and quering DB in order to reduce coupling
        // TODO: Add exception handling
        [HttpGet("{year}/{month}")]
        public async Task<IActionResult> Download(int year, int month, CancellationToken cancellationToken)
        {
            // TODO: What result ?
             await _reportProvider.CreateReportAsync(year, month, cancellationToken);
            
            // report.Save();
            
            // Return stream instead of reading bytes
            byte[] file = await System.IO.File.ReadAllBytesAsync("D:\\report.txt", cancellationToken);
            var response = File(file, "application/octet-stream", "report.txt");
            
            return response;
        }
        
       
    }
}
