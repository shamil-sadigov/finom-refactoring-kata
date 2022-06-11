using System.Threading.Tasks;

namespace ReportService.Services.Report;

public interface IReportService
{
    /// <returns>Address of created report</returns>
    Task<ReportLocation> CreateReportAsync(AccountingReportParams @params);
}