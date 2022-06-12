namespace ReportService.Application.Resolvers.BuhCodeResolver;

public interface IEmployeeBuhCodeResolver
{
    Task<string> GetEmployeeBuhcodeAsync(string employeeInn, CancellationToken cancellationToken);
}