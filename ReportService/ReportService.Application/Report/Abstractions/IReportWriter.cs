namespace ReportService.Application.Report.Abstractions;

public interface IReportWriter
{
    /// <summary>
    /// Writes report to <param name="textWriter"></param>
    /// </summary>
    public Task WriteAsync(
        TextWriter textWriter,
        int year,
        int month,
        IReadOnlyCollection<EmployeeReportableModel> employees);
}