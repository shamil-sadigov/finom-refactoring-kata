namespace ReportService.Application.Report;

/// <summary>
/// Just an abstract Report.
/// It can be a report stored in local file system, or in Redis, or in AWS S3 or somewhere else. 
/// </summary>
public abstract class Report
{
    protected Report(string fileName)
    {
        fileName.ThrowIfNull();
        FileName = fileName;
    }

    /// <summary>
    /// Name and extension of report file
    /// TODO: It's better to make it ValueObject
    /// </summary>
    /// <example>somereport-2020.txt</example>
    public string FileName { get; }
    
    public abstract Stream AsStream();
    public abstract Task<byte[]> AsBytesAsync();
    public abstract Task<string> AsTextAsync();
}