using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ReportService.Domain;

namespace ReportService.Services
{
    // TODO: Use record instead of class when switching to .NET 5
    public class GetEmployeeSalaryRequest
    {
        public GetEmployeeSalaryRequest(string buhCode)
        {
            BuhCode = buhCode;
        }

        public string BuhCode { get; set; }
    }
    
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
            
            // TODO: Стоит перейти yf .NET 5 ибо там не нужны все эти танцы для того чтобы сделать Post запрос, 
            
            var jsonContent = new StringContent(request.ToJson());

            var response = await _httpClient.PostAsync("/api/empcode/" + employee.Inn, jsonContent, token);

            response.EnsureSuccessStatusCode();

            var salaryString = await response.Content.ReadAsStringAsync();
            
            if (!int.TryParse(salaryString, out var parsedSalary))
            {
                throw new InvalidOperationException("Returned salary has invalid format");
            }

            return parsedSalary;
        }
    }
}