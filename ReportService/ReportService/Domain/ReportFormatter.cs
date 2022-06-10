using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportService.Domain
{
    // TODO: Для посмотроения отчетов возможно стоит задуматься от паттерне Builder
    public class ReportFormatter
    {
        public ReportFormatter(Employee e)
        {
            Employee = e;
        }

        public Action<Employee, Report> NL = (e, s) => s.S = s.S + Environment.NewLine;
        public Action<Employee, Report> WL = (e, s) => s.S = s.S + "--------------------------------------------";
        public Action<Employee, Report> WT = (e, s) => s.S = s.S + "         ";
        public Action<Employee, Report> WE = (e, s) => s.S = s.S + e.Name;
        public Action<Employee, Report> WS = (e, s) => s.S = s.S + e.Salary + "р";
        public Action<Employee, Report> WD = (e, s) => s.S = s.S + e.Department;
        
        // TODO: Это нигде не используется, вон из класса!
        public Employee Employee { get; }
    }
}
