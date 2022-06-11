using System;
using System.IO;

namespace ReportService.Services.Report;

public class ReportLocationProvider : IReportLocationProvider
{
    /// <summary>
    /// Builds and returns path where report should be uploaded.
    /// </summary>
    public string GetReportLocation(int year, int month)
    {
        ValidateArguments(year, month);

        var fileName = $"accounting-report-{year}-{month}.txt";

        var destinationPath = Path.Combine(
            Directory.GetCurrentDirectory(), 
            "reports", 
            year.ToString(),
            fileName);

        // destination path looks like => '..\reports\2018\accounting-report-2018-05.txt'
        return destinationPath;
    }

    private static void ValidateArguments(int year, int month)
    {
        if (year <= 0)
            throw new ArgumentOutOfRangeException(nameof(year), "Should be greater than zero");

        if (month is > 12 or < 1)
            throw new ArgumentOutOfRangeException(nameof(month), "Should be in range 1-12");
    }
}