using ReportService.Application.Report.Abstractions;

namespace ReportService.Application.Report;

public sealed class ReportGenerator:IReportGenerator
{
    // Возможно эта абстракция лишняя
    private readonly IReportInfoProvider _reportInfoProvider;
    private readonly IReportWriter _reportWriter;

    public ReportGenerator(
        IReportInfoProvider reportInfoProvider, 
        IReportWriter reportWriter)
    {
        _reportInfoProvider = reportInfoProvider;
        _reportWriter = reportWriter;
    }
    
    // Test cases:
    // - Create report and get it from location and validate
    // - Test when report is requested 2 times, then 2 time should not be calculated
    
    public async Task<Report> GenerateReportAsync(AccountingReportParams @params)
    {
        var (year, month, employees) = @params;
        
        ReportInfo report = _reportInfoProvider.GetReportInfo(year, month);

        if (File.Exists(report.FileName))
            return new LocalFileReport(report.Location, report.FileName);
        
        await WriteReportAsync();

        return new LocalFileReport(report.Location, report.FileName);
        
        async Task WriteReportAsync()
        {
            await using var streamWriter = File.CreateText(report.Location);
            await _reportWriter.WriteAsync(streamWriter, year, month, employees);
        }
    }
}