using ReportService.Application;

namespace ReportService.Tests.ReportService.Helpers;

public class InMemoryRepository:IEmployeeRepository
{
    private readonly IReadOnlyList<EmployeeModel> _readOnlyList;

    private InMemoryRepository(IReadOnlyList<EmployeeModel> readOnlyList) 
        => _readOnlyList = readOnlyList;

    public Task<IReadOnlyList<EmployeeModel>> GetAllAsync(CancellationToken cancellationToken) 
        => Task.FromResult(_readOnlyList);

    public static InMemoryRepository WithCollection(IReadOnlyList<EmployeeModel> employeeModels) 
        => new(employeeModels);
}