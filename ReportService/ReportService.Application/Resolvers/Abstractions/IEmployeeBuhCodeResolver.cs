namespace ReportService.Application.Resolvers.Abstractions;

public interface IEmployeeBuhCodeResolver
{
    Task<string> GetEmployeeBuhcodeAsync(string employeeInn, CancellationToken cancellationToken);
}