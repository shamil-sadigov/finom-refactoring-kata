namespace ReportService.Application.Report.Abstractions;

// TODO: Register in DI Container
public interface IReportInfoProvider
{
    ReportInfo GetReportInfo(int year, int month);
}