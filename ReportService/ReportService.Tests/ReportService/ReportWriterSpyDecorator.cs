using ReportService.Application;
using ReportService.Application.Report.Abstractions;

namespace ReportService.Tests.ReportService;

public class ReportWriterSpyDecorator:IReportWriter
{
    public int ReportGeneratedCount { get; private set; }

    private readonly IReportWriter _decoratee;

    public ReportWriterSpyDecorator(IReportWriter decoratee)
    {
        _decoratee = decoratee;
    }
    
    public async Task WriteAsync(
        TextWriter textWriter,
        int year,
        int month,
        IReadOnlyCollection<EmployeeReportItem> employees)
    {
        await _decoratee.WriteAsync(textWriter, year, month, employees);
        ++ReportGeneratedCount;
    }
}