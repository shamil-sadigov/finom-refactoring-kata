namespace ReportService.Application.Report;

public sealed class ReportInfo
{
    public ReportInfo(FileLocation location, FileName fileName, bool reportExists)
    {
        FileName = fileName;
        Location = location;
        
        ReportExists = reportExists;
    }

    public FileLocation Location { get; }
    public FileName FileName { get; }
    public bool ReportExists { get; }

    public static ReportInfo NewReportInfo(string location, string fileName) 
        => new(location, fileName, reportExists: false);

    public static ReportInfo ExistingReportInfo(string location, string fileName) => 
        new(location, fileName, reportExists: true);
}