namespace ReportService.Application;

public interface IEmployeeRepository
{
    Task<IReadOnlyList<EmployeeModel>> GetAllAsync(CancellationToken cancellationToken);
}