namespace ReportService.Application
{
    public sealed record EmployeeReportItem(string Name, string Inn, string Department, int Salary);

    public sealed record EmployeeDataModel(string Name, string Inn, string Department);
}
