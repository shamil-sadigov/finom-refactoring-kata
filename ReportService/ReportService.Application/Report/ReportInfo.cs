namespace ReportService.Application.Report;

public sealed class ReportInfo
{
    public ReportInfo(string location, string fileName)
    {
        location.ThrowIfNull();
        
        // TODO: Primitive obsession.
        fileName.ThrowIfNull();
        
        FileName = fileName;
        Location = location;
    }

    public string Location { get; }
    public string FileName { get; }
}