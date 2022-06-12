namespace ReportService.Application.Report;

public readonly struct FileLocation
{
    public FileLocation(string value)
    {
        value.ThrowIfNull();
        
        if (!Path.IsPathFullyQualified(value))
            throw new ArgumentException("File should have valid path format", nameof(value));

        Value = value;
    }

    public string Value { get; }

    public static implicit operator string(FileLocation fileLocation) => fileLocation.Value;
    public static implicit operator FileLocation(string fileLocation) => new(fileLocation);
}