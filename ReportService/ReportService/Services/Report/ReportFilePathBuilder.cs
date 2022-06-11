using System.IO;

namespace ReportService.Services.Report;

public class ReportFilePathBuilder : IReportFilePathBuilder
{
    public string GetFilePath(int year, int month)
    {
        string fileName = $"accounting-report-{year}-{month}.txt";

        var destinationFile = Path.Combine(
            Directory.GetCurrentDirectory(), 
            "reports", 
            year.ToString(),
            fileName);

        // destination file looks like => '..\reports\2018\accounting-report-2018-05'
        return destinationFile;
    }
}