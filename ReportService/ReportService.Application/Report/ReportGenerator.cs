using ReportService.Application.Report.Abstractions;

namespace ReportService.Application.Report;

public sealed class ReportGenerator:IReportGenerator
{
    // Возможно эта абстракция лишняя
    private readonly IReportLocationProvider _pathProvider;
    private readonly IReportWriter _reportWriter;

    public ReportGenerator(
        IReportLocationProvider pathProvider, 
        IReportWriter reportWriter)
    {
        _pathProvider = pathProvider;
        _reportWriter = reportWriter;
    }
    
    // Test cases:
    // - Create report and get it from location and validate
    // - Test when report is requested 2 times, then 2 time should not be calculated
    
    public async Task<ReportLocation> GenerateReportAsync(AccountingReportParams @params)
    {
        var (year, month, employees) = @params;
        
        string reportLocation = _pathProvider.GetReportLocation(year, month);

        if (File.Exists(reportLocation))
            return new ReportLocation(ReportLocationType.FileSystem, reportLocation);
        
        await CreateReportAsync();

        return new ReportLocation(ReportLocationType.FileSystem, reportLocation);
        
        async Task CreateReportAsync()
        {
            await using var streamWriter = File.CreateText(reportLocation);
            await _reportWriter.WriteAsync(streamWriter, year, month, employees);
        }
    }
}