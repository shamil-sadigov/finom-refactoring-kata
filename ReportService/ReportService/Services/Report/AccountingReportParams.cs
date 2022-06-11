using System.Collections.Generic;
using ReportService.Domain;

namespace ReportService.Services.Report;

public record AccountingReportParams(int Year, int Month, IReadOnlyCollection<EmployeeReportItem> Employees);