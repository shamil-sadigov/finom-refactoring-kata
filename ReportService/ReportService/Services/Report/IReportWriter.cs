using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ReportService.Domain;

namespace ReportService.Services.Report;

// TODO: Register in DI container
public interface IReportWriter
{
    public Task WriteReportAsync(
        StreamWriter streamWriter,
        int year,
        int month,
        IReadOnlyCollection<EmployeeReportItem> employees);
}