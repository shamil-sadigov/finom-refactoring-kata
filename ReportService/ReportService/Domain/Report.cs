namespace ReportService.Domain
{
    public class Report
    {
        public string Content { get; set; }
        
        public void Save()
        {
            // TODO: Стоит вынести в конфиг
            System.IO.File.WriteAllText("D:\\report.txt", Content);
        }
    }
}
