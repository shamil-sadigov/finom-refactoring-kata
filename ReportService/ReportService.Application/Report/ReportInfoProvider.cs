using ReportService.Application.Report.Abstractions;

namespace ReportService.Application.Report;

public class ReportInfoProvider : IReportInfoProvider
{
    private readonly string _reportDirectoryRoot;

    public ReportInfoProvider(string reportDirectoryRoot)
    {
        reportDirectoryRoot.ThrowIfNull();
        
        if (!Directory.Exists(reportDirectoryRoot))
            throw new DirectoryNotFoundException($"Directory '{reportDirectoryRoot}' is not found");

        _reportDirectoryRoot = reportDirectoryRoot;
    }
    
    public ReportInfo GetReportInfo(int year, int month)
    {
        ValidateArguments(year, month);

        var reportFileName = $"accounting-report-{year}-{month}.txt";
        var reportDirectory = ProvideReportDirectory(year);

        // reportLocation looks like => '..\reports\accounting-report-2018-05.txt'
        var reportLocation = Path.Combine(reportDirectory, reportFileName);
        
        if(File.Exists(reportLocation))
            return ReportInfo.ForExistingReport(reportLocation, reportFileName);
        
        return ReportInfo.ForNewReport(reportLocation, reportFileName);
    }

    private string ProvideReportDirectory(int year)
    {
        var reportDirectory = Path.Combine(
            _reportDirectoryRoot,
            year.ToString());

        if (!Directory.Exists(reportDirectory))
            Directory.CreateDirectory(reportDirectory);
        
        return reportDirectory;
    }

    private static void ValidateArguments(int year, int month)
    {
        if (year <= 0)
            throw new ArgumentOutOfRangeException(nameof(year), "Should be greater than zero");

        if (month is > 12 or < 1)
            throw new ArgumentOutOfRangeException(nameof(month), "Should be in range 1-12");
    }
}