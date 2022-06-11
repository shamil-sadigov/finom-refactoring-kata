using System.IO;
using System.Threading.Tasks;

namespace ReportService.Services.Report;

public sealed class ReportService:IReportService
{
    // Возможно эта абстракция лишняя
    private readonly IReportLocationProvider _pathProvider;
    private readonly IReportWriter _reportWriter;

    public ReportService(
        IReportLocationProvider pathProvider, 
        IReportWriter reportWriter)
    {
        _pathProvider = pathProvider;
        _reportWriter = reportWriter;
    }
    
    public async Task<ReportLocation> CreateReportAsync(AccountingReportParams @params)
    {
        var (year, month, employees) = @params;
        
        var reportFilePath = _pathProvider.GetReportLocation(year, month);

        await using var streamWriter = File.CreateText(reportFilePath);
        
        await _reportWriter.WriteReportAsync(streamWriter, year, month, employees);

        return new ReportLocation(ReportLocationType.FileSystem, reportFilePath);
    }
}