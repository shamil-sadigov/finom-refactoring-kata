namespace ReportService.Application.Report;

public class Report
{
    public Report(string fileName, string location)
    {
        ValidateArguments(fileName, location);

        FileName = fileName;
        _reportLocation = location;
    }
    
    /// <summary>
    /// Name and extension of report file
    /// TODO: It's better to make it ValueObject
    /// </summary>
    /// <example>somereport-2020.txt</example>
    public string FileName { get; }
    
    private readonly string _reportLocation;
    
    public Stream AsStream() => File.OpenRead(_reportLocation);
    
    public Task<string> AsTextAsync() => File.ReadAllTextAsync(_reportLocation);
    
    private static void ValidateArguments(string fileName, string location)
    {
        fileName.ThrowIfNull();
        location.ThrowIfNull();

        if (!Path.HasExtension(location))
            throw new ArgumentException("Should have extensions", nameof(location));

        if (!File.Exists(location))
            throw new ArgumentException("Such report file doesn't exists", nameof(location));
    }
}