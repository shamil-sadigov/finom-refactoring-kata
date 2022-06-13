namespace ReportService.Application;

/// <summary>
/// Reports are generated based on this object
/// </summary>
public sealed record EmployeeReportableModel(string Name, string Inn, string Department, int Salary);

/// <summary>
/// Employees are retrieved from database in this form
/// </summary>
public sealed record EmployeeDataModel(string Name, string Inn, string Department);