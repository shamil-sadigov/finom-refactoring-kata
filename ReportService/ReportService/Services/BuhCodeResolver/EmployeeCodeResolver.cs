using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ReportService.Services.BuhCodeResolver;

public class EmployeeCodeResolver:IEmployeeCodeResolver
{
    // TODO: Есть предположение что на каждый inn будет всегда возвращаться один и тот же статичный Code 
    // поэтому стоит задуматься о кешировании
    public async Task<string> GetEmployeeBuhcodeAsync(string employeeInn, CancellationToken cancellationToken)
    {
            
        // TODO: Создавать каждый раз новый HttpClient дорого, к тому же он разделяемый и потокобезопасный
        // стоит вынести в статичное поле
        var client = new HttpClient();
        return await client.GetStringAsync("http://buh.local/api/inn/" + employeeInn, cancellationToken);
    }
}