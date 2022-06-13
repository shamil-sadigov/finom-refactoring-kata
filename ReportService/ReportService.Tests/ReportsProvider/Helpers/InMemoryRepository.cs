using ReportService.Application;

namespace ReportService.Tests.ReportsProvider.Helpers;

public class InMemoryRepository:IEmployeeRepository
{
    private readonly IReadOnlyList<EmployeeDataModel> _readOnlyList;

    private InMemoryRepository(IReadOnlyList<EmployeeDataModel> readOnlyList) 
        => _readOnlyList = readOnlyList;

    public Task<IReadOnlyList<EmployeeDataModel>> GetAllAsync(CancellationToken cancellationToken) 
        => Task.FromResult(_readOnlyList);

    public static InMemoryRepository WithCollection(IReadOnlyList<EmployeeDataModel> employeeModels) 
        => new(employeeModels);
}