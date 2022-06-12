namespace ReportService.Application.Report.Abstractions;

// TODO: Add DI container
public interface IReportProvider
{
    Task<Report> CreateReportAsync(int year, int month, CancellationToken cancellationToken);
}