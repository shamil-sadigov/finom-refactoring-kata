using System;

namespace ReportService.Domain
{
    // TODO: Для посмотроения отчетов возможно стоит задуматься от паттерне Builder
    public class ReportFormatter
    {
        public ReportFormatter(Employee e)
        {
            Employee = e;
        }

        public Action<Employee, Report> NL = (e, s) => s.Content = s.Content + Environment.NewLine;
        public Action<Employee, Report> WL = (e, s) => s.Content = s.Content + "--------------------------------------------";
        public Action<Employee, Report> WT = (e, s) => s.Content = s.Content + "         ";
        public Action<Employee, Report> WE = (e, s) => s.Content = s.Content + e.Name;
        public Action<Employee, Report> WS = (e, s) => s.Content = s.Content + e.Salary + "р";
        public Action<Employee, Report> WD = (e, s) => s.Content = s.Content + e.Department;
        
        // TODO: Это нигде не используется, вон из класса!
        public Employee Employee { get; }
    }
}
