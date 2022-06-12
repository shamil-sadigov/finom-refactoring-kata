namespace ReportService.Application.Resolvers.SalaryResolver
{
    // TODO: Вынести это в DI
    // TODO: А что если завтра мы решим брать salary не из удаленной службы а из локальной реплики ?
    
    public interface IEmployeeSalaryResolver
    {
        Task<int> GetSalaryAsync(
            string employeeBuhCode, 
            string employeeInn, 
            CancellationToken token);
    }
}