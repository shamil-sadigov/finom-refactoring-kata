using System.Threading;
using System.Threading.Tasks;
using ReportService.Domain;

namespace ReportService.Services
{
    // TODO: Вынести это в DI
    // TODO: А что если завтра мы решим брать salary не из удаленной службы а из локальной реплики ?
    
    public interface IEmployeeSalaryProvider
    {
        Task<int> GetSalaryAsync(Employee employee, CancellationToken token);
    }
}