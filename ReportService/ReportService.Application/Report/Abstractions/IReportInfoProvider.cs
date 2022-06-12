namespace ReportService.Application.Report.Abstractions;

public interface IReportInfoProvider
{
    ReportInfo GetReportInfo(int year, int month);
}