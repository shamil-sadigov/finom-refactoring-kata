using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ReportService.Domain;

namespace ReportService.Services.Report;

// TODO: Register in DI container
public interface IReportWriter
{
    /// <summary>
    /// Writes report to <param name="textWriter"></param>
    /// </summary>
    public Task WriteAsync(
        TextWriter textWriter,
        int year,
        int month,
        IReadOnlyCollection<EmployeeReportItem> employees);
}