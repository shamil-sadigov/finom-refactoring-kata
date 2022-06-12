using ReportService.Application.Report.Abstractions;

namespace ReportService.Application.Report;

public class ReportProvider
{
    private readonly EmployeeModelTransformation _employeeModelTransformation;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IReportGenerator _reportGenerator;

    public ReportProvider(
        IReportGenerator reportGenerator, 
        EmployeeModelTransformation employeeModelTransformation, 
        IEmployeeRepository employeeRepository)
    {
        _reportGenerator = reportGenerator;
        _employeeModelTransformation = employeeModelTransformation;
        _employeeRepository = employeeRepository;
    }

    public async Task<Report> CreateReportAsync(int year, int month, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        IReadOnlyList<EmployeeModel> employees = 
            await _employeeRepository.GetAllAsync(cancellationToken);

        EmployeeReportItem[] employeeReportItems = 
            await _employeeModelTransformation.TransformToReportableItemsAsync(employees, cancellationToken);
            
        Report report = 
            await _reportGenerator.GenerateReportAsync(new AccountingReportParams(year, month, employeeReportItems));

        return report;
    }
}