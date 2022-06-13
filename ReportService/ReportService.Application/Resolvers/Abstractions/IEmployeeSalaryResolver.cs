namespace ReportService.Application.Resolvers.Abstractions;

public interface IEmployeeSalaryResolver
{
    Task<int> GetSalaryAsync(
        string employeeBuhCode, 
        string employeeInn, 
        CancellationToken token);
}