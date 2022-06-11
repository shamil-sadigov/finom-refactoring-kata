using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ReportService.Services.SalaryProvider
{
    public class EmployeeSalaryProvider : IEmployeeSalaryProvider
    {
        private readonly HttpClient _httpClient;

        public EmployeeSalaryProvider()
        {
            // TODO: Extract uri to appsettings
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://salary.local", UriKind.Absolute)
            };
        }
        
        public async Task<int> GetSalaryAsync(
            string employeeBuhCode, 
            string employeeInn, 
            CancellationToken token)
        {
            // Add null checks
            
            var request = new GetEmployeeSalaryRequest(employeeBuhCode);
            
            var response = await _httpClient.PostAsJsonAsync("/api/empcode/" + employeeInn, request, token);

            response.EnsureSuccessStatusCode();

            var salaryString = await response.Content.ReadAsStringAsync(token);
            
            if (!int.TryParse(salaryString, out var parsedSalary))
                throw new InvalidOperationException("Returned salary has invalid format");

            return parsedSalary;
        }
    }
}