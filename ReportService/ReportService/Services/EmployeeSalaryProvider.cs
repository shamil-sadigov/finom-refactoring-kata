using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using ReportService.Domain;

namespace ReportService.Services
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
        
        public async Task<int> GetSalaryAsync(Employee employee, CancellationToken token)
        {
            var request = new GetEmployeeSalaryRequest(employee.BuhCode);
            
            var response = await _httpClient.PostAsJsonAsync("/api/empcode/" + employee.Inn, request, token);

            response.EnsureSuccessStatusCode();

            var salaryString = await response.Content.ReadAsStringAsync(token);
            
            if (!int.TryParse(salaryString, out var parsedSalary))
                throw new InvalidOperationException("Returned salary has invalid format");

            return parsedSalary;
        }
    }
}