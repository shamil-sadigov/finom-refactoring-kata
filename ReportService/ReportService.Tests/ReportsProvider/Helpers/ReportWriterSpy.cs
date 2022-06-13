using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ReportService.Application;
using ReportService.Application.Report.Abstractions;

namespace ReportService.Tests.ReportService.Helpers;

public class ReportWriterSpy:IReportWriter
{
    public int ReportGeneratedCount { get; private set; }

    private readonly IReportWriter _decoratee;

    public ReportWriterSpy(IReportWriter decoratee)
    {
        _decoratee = decoratee;
    }
    
    public async Task WriteAsync(
        TextWriter textWriter,
        int year,
        int month,
        IReadOnlyCollection<EmployeeReportableModel> employees)
    {
        await _decoratee.WriteAsync(textWriter, year, month, employees);
        ++ReportGeneratedCount;
    }
}