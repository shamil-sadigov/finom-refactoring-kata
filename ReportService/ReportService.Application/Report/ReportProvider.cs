using ReportService.Application.Report.Abstractions;

namespace ReportService.Application.Report;

public class ReportProvider
{
    private readonly EmployeeModelTransformation _employeeModelTransformation;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IReportInfoProvider _reportInfoProvider;
    private readonly IReportWriter _reportWriter;

    public ReportProvider(
        EmployeeModelTransformation employeeModelTransformation, 
        IEmployeeRepository employeeRepository,
        IReportInfoProvider reportInfoProvider,
        IReportWriter reportWriter)
    {
        _employeeModelTransformation = employeeModelTransformation;
        _employeeRepository = employeeRepository;
        _reportInfoProvider = reportInfoProvider;
        _reportWriter = reportWriter;
    }

    // - Test when report is requested 2 times, then 2 time should not be calculated
    
    public async Task<Report> CreateReportAsync(int year, int month, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        ReportInfo reportInfo = _reportInfoProvider.GetReportInfo(year, month);
        
        if (ReportWasAlreadyCreatedPreviously(reportInfo, out var cachedReport)) 
            return cachedReport!;

        return await CreateNewReport(reportInfo, year, month, cancellationToken);
    }

    private async Task<Report> CreateNewReport(
        ReportInfo reportInfo,
        int year, 
        int month, 
        CancellationToken cancellationToken)
    {
        IReadOnlyList<EmployeeModel> employees = 
            await _employeeRepository.GetAllAsync(cancellationToken);

        EmployeeReportItem[] employeeReportItems =
            await _employeeModelTransformation.TransformToReportableItemsAsync(employees, cancellationToken);
        
        await using var reportStream = File.CreateText(reportInfo.Location);
        await _reportWriter.WriteAsync(reportStream, year, month, employeeReportItems);
        
        return new Report(reportInfo.Location, reportInfo.FileName);
    }

    private static bool ReportWasAlreadyCreatedPreviously(ReportInfo reportInfo, out Report? localFileReport)
    {
        if (reportInfo.ReportExists)
        {
            localFileReport = new Report(reportInfo.Location, reportInfo.FileName);
            return true;
        }

        localFileReport = null;
        return false;
    }
}