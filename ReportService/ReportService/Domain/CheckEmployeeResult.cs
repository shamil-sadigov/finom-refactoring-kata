using System;
using System.Collections.Generic;

namespace ReportService.Domain
{
    public class CheckPayrollResult
    {
        public string ErrorCode { get; set; }
        public List<CheckEmployeeInPayrollResult> RecipientErrors { get; set; }
    }

    public class CheckEmployeeInPayrollResult
    {
        public Guid Id { get; set; }
        public string ErrorCode { get; set; }
    }
}
