namespace ReportService.Application;

public interface IEmployeeRepository
{
    /// <summary>
    /// Returns all employees from database
    /// </summary>
    Task<IReadOnlyList<EmployeeDataModel>> GetAllAsync(CancellationToken cancellationToken);
}