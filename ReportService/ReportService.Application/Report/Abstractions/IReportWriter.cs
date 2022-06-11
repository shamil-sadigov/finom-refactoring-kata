namespace ReportService.Application.Report.Abstractions;

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