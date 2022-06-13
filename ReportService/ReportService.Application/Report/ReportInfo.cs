using ReportService.Application.Report.ValueObjects;

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

    public static ReportInfo ForNewReport(string location, string fileName) 
        => new(location, fileName, reportExists: false);

    public static ReportInfo ForExistingReport(string location, string fileName) => 
        new(location, fileName, reportExists: true);
}