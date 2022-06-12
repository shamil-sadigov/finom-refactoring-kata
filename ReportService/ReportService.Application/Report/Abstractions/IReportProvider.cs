namespace ReportService.Application.Report.Abstractions;

public interface IReportProvider
{
    Task<Report> CreateReportAsync(int year, int month, CancellationToken cancellationToken);
}