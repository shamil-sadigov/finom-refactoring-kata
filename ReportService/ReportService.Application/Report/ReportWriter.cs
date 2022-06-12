using System.Globalization;
using ReportService.Application.Report.Abstractions;

namespace ReportService.Application.Report;

public sealed class ReportWriter:IReportWriter
{
    private readonly string _dashedLine = new('-', 20);
    
    public readonly CultureInfo ReportCulture = new("ru-ru");
    
    public async Task WriteAsync(
        TextWriter textWriter, 
        int year,
        int month, 
        IReadOnlyCollection<EmployeeReportableModel> employees)
    {
        await WriteHeaderAsync(textWriter, year, month);
        
        var organizationSalarySum = await WriteBodyAsync(textWriter, employees);
        
        await WriteFooterAsync(textWriter, organizationSalarySum);
    }

    private async Task WriteHeaderAsync(TextWriter streamWriter, int year, int month)
    {
        var yearAndMonth = new DateTime(year, month, 1).ToString("Y", ReportCulture);

        await streamWriter.WriteLineAsync(yearAndMonth);
        await streamWriter.WriteLineAsync(_dashedLine);
    }
    
    private async Task<int> WriteBodyAsync(
        TextWriter streamWriter, 
        IReadOnlyCollection<EmployeeReportableModel> employees)
    {
        var organizationSalarySum = 0;
        
        foreach (var department in employees.GroupBy(x => x.Department))
        {
            var departmentSalarySum = 0;

            var departmentName = department.Key;
            await streamWriter.WriteLineAsync(departmentName);

            foreach (var employee in department)
            {
                await streamWriter.WriteLineAsync($"{employee.Name} {employee.Salary.ToString("C0", ReportCulture)}");
                departmentSalarySum += employee.Salary;
            }

            await streamWriter.WriteLineAsync();
            await streamWriter.WriteLineAsync($"Всего по отделу {departmentSalarySum.ToString("C0", ReportCulture)}");
            await streamWriter.WriteLineAsync(_dashedLine);

            organizationSalarySum += departmentSalarySum;
        }
        
        return organizationSalarySum;
    }
    
    private async Task WriteFooterAsync(TextWriter streamWriter, int organizationSalarySum)
    {
        await streamWriter.WriteAsync("Всего по предприятию " + organizationSalarySum.ToString("C0", ReportCulture));
    }
}