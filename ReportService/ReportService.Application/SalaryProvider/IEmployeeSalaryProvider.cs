namespace ReportService.Application.SalaryProvider
{
    // TODO: Вынести это в DI
    // TODO: А что если завтра мы решим брать salary не из удаленной службы а из локальной реплики ?
    
    public interface IEmployeeSalaryProvider
    {
        Task<int> GetSalaryAsync(
            string employeeBuhCode, 
            string employeeInn, 
            CancellationToken token);
    }
}