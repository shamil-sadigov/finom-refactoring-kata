namespace ReportService.Application.Report.Abstractions;

public record AccountingReportParams(int Year, int Month, IReadOnlyCollection<EmployeeReportItem> Employees);

public interface IReportGenerator
{
    /// <returns>Address of created report</returns>
    Task<Report> GenerateReportAsync(AccountingReportParams @params);
}