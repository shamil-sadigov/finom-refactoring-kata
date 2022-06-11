using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ReportService.Domain;

namespace ReportService.Services.Report;

public sealed class ReportWriter:IReportWriter
{
    private readonly string _dashedLine = new('-', 20);
    
    
    public async Task WriteReportAsync(
        TextWriter textWriter, 
        int year,
        int month, 
        IReadOnlyCollection<EmployeeReportItem> employees)
    {
        await WriteHeaderAsync(textWriter, year, month);
        
        var organizationSalarySum = await WriteBodyAsync(textWriter, employees);
        
        await WriteFooterAsync(textWriter, organizationSalarySum);
    }

    private async Task WriteHeaderAsync(TextWriter streamWriter, int year, int month)
    {
        var yearAndMonth = new DateTime(year, month, 0).ToString("Y");

        await streamWriter.WriteLineAsync(yearAndMonth);
        await streamWriter.WriteLineAsync(_dashedLine);
    }
    
    private async Task<int> WriteBodyAsync(
        TextWriter streamWriter, 
        IReadOnlyCollection<EmployeeReportItem> employees)
    {
        int organizationSalarySum = 0;
        
        foreach (var department in employees.GroupBy(x => x.Department))
        {
            var departmentSalarySum = 0;

            var departmentName = department.Key;
            await streamWriter.WriteLineAsync(departmentName);

            foreach (var employee in department)
            {
                await streamWriter.WriteAsync(employee.Name);
                await streamWriter.WriteAsync(' ');
                await streamWriter.WriteLineAsync(employee.Salary.ToString());

                departmentSalarySum += employee.Salary;
            }

            await streamWriter.WriteAsync("Всего по отделу ");
            await streamWriter.WriteLineAsync(departmentSalarySum.ToString());
            await streamWriter.WriteLineAsync(_dashedLine);

            organizationSalarySum += departmentSalarySum;
        }
        
        return organizationSalarySum;
    }
    
    private async Task WriteFooterAsync(TextWriter streamWriter, int organizationSalarySum)
    {
        await streamWriter.WriteLineAsync("Всего по предприятию " + organizationSalarySum);
    }
}