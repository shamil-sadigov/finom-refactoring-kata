namespace ReportService.Application.Report;

public class Report
{
    public Report(FileName fileName, FileLocation location)
    {
        if (!File.Exists(location))
            throw new ArgumentException("Such report file doesn't exists", nameof(location));
        
        FileName = fileName;
        _reportLocation = location;
    }
    
    public FileName FileName { get; }
    private readonly FileLocation _reportLocation;
    
    public Stream AsStream() => File.OpenRead(_reportLocation);
    
    public Task<string> AsTextAsync() => File.ReadAllTextAsync(_reportLocation);
}