using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReportService.Application.Report.Abstractions;

namespace ReportService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        private readonly IReportProvider _reportProvider;

        public ReportController(IReportProvider reportProvider)
        {
            _reportProvider = reportProvider;
        }
        
        [HttpGet("{year:int}/{month:int}")]
        public async Task<IActionResult> Download(int year, int month, CancellationToken cancellationToken)
        {
            var report = await _reportProvider.CreateReportAsync(year, month, cancellationToken);
            
            return File(report.AsStream(), "application/octet-stream", report.FileName);
        }
    }
}
