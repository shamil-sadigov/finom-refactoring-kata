using System;

namespace ReportService.Domain
{
    public class UpsertPayrollReceived
    {
        public bool IsNew { get; set; }
        public PayrollInLayer Payroll { get; set; }
        public Guid CompanyId { get; set; }
        public Guid UserId { get; set; }
    }
}
