namespace ReportService.Application.Report;

public sealed class ReportInfo
{
    public ReportInfo(string location, string fileName, bool reportExists)
    {
        location.ThrowIfNull();
        
        // TODO: Primitive obsession.
        fileName.ThrowIfNull();
        
        FileName = fileName;
        Location = location;
        
        ReportExists = reportExists;
    }

    public string Location { get; }
    public string FileName { get; }
    public bool ReportExists { get; }

    public static ReportInfo NewReportInfo(string location, string fileName) =>
        new(location, fileName, reportExists: false);

    public static ReportInfo ExistingReportInfo(string location, string fileName) => 
        new(location, fileName, reportExists: true);
}