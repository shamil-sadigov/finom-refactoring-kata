using System.Threading;
using System.Threading.Tasks;

namespace ReportService.Services.SalaryProvider
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