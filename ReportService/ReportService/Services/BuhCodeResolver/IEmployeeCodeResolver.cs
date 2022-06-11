using System.Threading;
using System.Threading.Tasks;

namespace ReportService.Services.BuhCodeResolver;

public interface IEmployeeCodeResolver
{
    Task<string> GetEmployeeBuhcodeAsync(string employeeInn, CancellationToken cancellationToken);
}