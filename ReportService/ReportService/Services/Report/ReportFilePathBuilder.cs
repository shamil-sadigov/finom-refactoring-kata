using System.IO;

namespace ReportService.Services.Report;

public class ReportFilePathBuilder : IReportFilePathBuilder
{
    public string GetFilePath(int year, int month)
    {
        string fileName = $"accounting-report-{year}-{month}";

        var destinationFile = Path.Combine(
            Directory.GetCurrentDirectory(), 
            "reports", 
            year.ToString(),
            fileName);

        return destinationFile;
    }
}