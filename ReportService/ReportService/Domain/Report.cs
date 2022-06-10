using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportService.Domain
{
    public class Report
    {
        // TODO: Переименовать
        public string S { get; set; }
        public void Save()
        {
            // TODO: Стоит вынести в конфиг
            System.IO.File.WriteAllText("D:\\report.txt", S);
        }
    }
}
