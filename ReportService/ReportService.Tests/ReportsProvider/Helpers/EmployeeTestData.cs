using System.Collections.Generic;
using System.Linq;
using ReportService.Application;

namespace ReportService.Tests.ReportService.Helpers;

public static class EmployeeTestData
{
    public static readonly List<EmployeeReportableModel> EmployeesReportItems = new()
    {
        new("Ernest Hemingway", "1", "Писатели", 2000),

        new("John Fogerty", "3", "Музыканты", 5000),
        new("David Gilmour", "4", "Музыканты", 5000),

        new("Charles Babbage", "6", "IT", 6000),
        new("Ada Lovelace", "7", "IT", 6000),
        new("Tim Berners Lee", "5", "IT", 8000),
    };
    
    public static List<EmployeeDataModel> EmployeeDataModels { get; }
        = EmployeesReportItems.Select(x => new EmployeeDataModel(x.Name, x.Inn, x.Department))
            .ToList();

    public static int GetSalaryByInn(string employeeInn)
    {    
        var employee = EmployeesReportItems.Single(x => x.Inn == employeeInn);
        return employee.Salary;
    }
}