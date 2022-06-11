namespace ReportService.Services.Report;

// TODO: Register in DI Container
public interface IReportFilePathBuilder
{
    string GetFilePath(int year, int month);
}