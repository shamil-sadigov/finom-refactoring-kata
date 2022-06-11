namespace ReportService.Application.BuhCodeResolver;

public interface IEmployeeCodeResolver
{
    Task<string> GetEmployeeBuhcodeAsync(string employeeInn, CancellationToken cancellationToken);
}