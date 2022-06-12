namespace ReportService.Application.Report;

public sealed class LocalFileReport:Report
{
    public LocalFileReport(string filePath, string fileName):base(fileName)
    {
        filePath.ThrowIfNull();

        if (!Path.HasExtension(filePath))
            throw new ArgumentException("Should have extensions", nameof(filePath));

        if (!File.Exists(filePath))
            throw new ArgumentException("Such report file doesn't exists", nameof(filePath));

        _filePath = filePath;
    }
    
    private readonly string _filePath;
    
    public override Stream AsStream() 
        => File.OpenRead(_filePath);

    public override Task<byte[]> AsBytesAsync() 
        => File.ReadAllBytesAsync(_filePath);

    public override Task<string> AsTextAsync() 
        => File.ReadAllTextAsync(_filePath);
}