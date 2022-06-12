namespace ReportService.Application;

public interface IEmployeeRepository
{
    Task<IReadOnlyList<EmployeeDataModel>> GetAllAsync(CancellationToken cancellationToken);
}