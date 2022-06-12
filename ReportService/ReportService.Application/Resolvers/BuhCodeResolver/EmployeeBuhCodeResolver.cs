namespace ReportService.Application.Resolvers.BuhCodeResolver;

public class EmployeeBuhCodeResolver:IEmployeeBuhCodeResolver
{
    private readonly HttpClient _httpClient;

    public EmployeeBuhCodeResolver(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<string> GetEmployeeBuhcodeAsync(string employeeInn, CancellationToken cancellationToken)
    {
        // BuhCode можно было бы кешировать дабы избежать последующих запросов,
        // но у меня не достаточно данных о том как часто buhCode изменяется,
        // поэтому оставим как есть
        
        return await _httpClient.GetStringAsync("/api/inn/" + employeeInn, cancellationToken);
    }
}