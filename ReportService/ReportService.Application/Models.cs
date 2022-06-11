namespace ReportService.Application
{
    public sealed record EmployeeReportItem(string Name, string Inn, string Department, int Salary);

    public sealed record EmployeeModel(string Name, string Inn, string Department);
}
