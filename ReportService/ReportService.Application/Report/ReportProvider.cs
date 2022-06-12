using ReportService.Application.Report.Abstractions;

namespace ReportService.Application.Report;

public class ReportProvider : IReportProvider
{
    private readonly EmployeeTransformation _employeeTransformation;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IReportInfoProvider _reportInfoProvider;
    private readonly IReportWriter _reportWriter;

    public ReportProvider(
        EmployeeTransformation employeeTransformation, 
        IEmployeeRepository employeeRepository,
        IReportInfoProvider reportInfoProvider,
        IReportWriter reportWriter)
    {
        _employeeTransformation = employeeTransformation;
        _employeeRepository = employeeRepository;
        _reportInfoProvider = reportInfoProvider;
        _reportWriter = reportWriter;
    }
    
    public async Task<Report> CreateReportAsync(int year, int month, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        ReportInfo reportInfo = _reportInfoProvider.GetReportInfo(year, month);
        
        if (ReportWasAlreadyCreatedEarlier(reportInfo, out var existingReport)) 
            return existingReport!;

        return await CreateNewReport(reportInfo, year, month, cancellationToken);
    }

    private async Task<Report> CreateNewReport(
        ReportInfo reportInfo,
        int year, 
        int month, 
        CancellationToken cancellationToken)
    {
        IReadOnlyList<EmployeeDataModel> employeesDataModels 
            = await _employeeRepository.GetAllAsync(cancellationToken);

        EmployeeReportableModel[] employeeReportableModels =
            await _employeeTransformation.TransformToReportableModelsAsync(employeesDataModels, cancellationToken);

        await using (var reportStream = File.CreateText(reportInfo.Location))
        {
            await _reportWriter.WriteAsync(reportStream, year, month, employeeReportableModels);
        }
        
        return new Report(reportInfo.FileName, reportInfo.Location);
    }

    private static bool ReportWasAlreadyCreatedEarlier(ReportInfo reportInfo, out Report? localFileReport)
    {
        if (reportInfo.ReportExists)
        {
            localFileReport = new Report(reportInfo.FileName, reportInfo.Location);
            return true;
        }

        localFileReport = null;
        return false;
    }
}