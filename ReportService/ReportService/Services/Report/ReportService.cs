using System.IO;
using System.Threading.Tasks;

namespace ReportService.Services.Report;

public sealed class ReportService:IReportService
{
    // Возможно эта абстракция лишняя
    private readonly IReportFilePathBuilder _pathBuilder;
    private readonly IReportWriter _reportWriter;

    public ReportService(
        IReportFilePathBuilder pathBuilder, 
        IReportWriter reportWriter)
    {
        _pathBuilder = pathBuilder;
        _reportWriter = reportWriter;
    }
    
    public async Task<ReportLocation> CreateReportAsync(AccountingReportParams @params)
    {
        var (year, month, employees) = @params;
        
        var reportFilePath = _pathBuilder.GetFilePath(year, month);

        await using var streamWriter = File.CreateText(reportFilePath);
        
        await _reportWriter.WriteReportAsync(streamWriter, year, month, employees);

        return new ReportLocation(ReportLocationType.FileSystem, reportFilePath);
    }
}