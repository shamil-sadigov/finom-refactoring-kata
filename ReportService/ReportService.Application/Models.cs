namespace ReportService.Application
{
    public sealed record EmployeeReportableModel(string Name, string Inn, string Department, int Salary);

    public sealed record EmployeeDataModel(string Name, string Inn, string Department);
}
