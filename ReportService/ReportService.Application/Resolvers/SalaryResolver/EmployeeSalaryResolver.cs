using System.Net.Http.Json;

namespace ReportService.Application.Resolvers.SalaryResolver
{
    public class EmployeeSalaryResolver : IEmployeeSalaryResolver
    {
        private readonly HttpClient _httpClient;

        public EmployeeSalaryResolver(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<int> GetSalaryAsync(
            string employeeBuhCode, 
            string employeeInn, 
            CancellationToken token)
        {
            employeeBuhCode.ThrowIfNull();
            employeeInn.ThrowIfNull();
            
            var request = new GetEmployeeSalaryRequest(employeeBuhCode);
            
            // вместо того чтобы получать ЗП сотрудника через синхронное http API
            // я бы предпочел подписаться на события удалленого сервиса и хранить локально
            // реплику всех зарплат сотрудников дабы избавиться от лишних http обращений.
            // но пока оставим так.
            
            var response = await _httpClient.PostAsJsonAsync("/api/empcode/" + employeeInn, request, token);

            response.EnsureSuccessStatusCode();

            var salaryString = await response.Content.ReadAsStringAsync(token);
            
            if (!int.TryParse(salaryString, out var parsedSalary))
                throw new InvalidOperationException(
                    $"Returned salary for buhCode '{employeeBuhCode}' has invalid format");

            return parsedSalary;
        }
    }
}