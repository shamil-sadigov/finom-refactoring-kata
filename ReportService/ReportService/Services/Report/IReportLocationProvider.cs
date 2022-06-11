namespace ReportService.Services.Report;

// TODO: Register in DI Container
public interface IReportLocationProvider
{
    string GetReportLocation(int year, int month);
}