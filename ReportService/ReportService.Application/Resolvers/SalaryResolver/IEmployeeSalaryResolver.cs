namespace ReportService.Application.Resolvers.SalaryResolver
{
    public interface IEmployeeSalaryResolver
    {
        Task<int> GetSalaryAsync(
            string employeeBuhCode, 
            string employeeInn, 
            CancellationToken token);
    }
}