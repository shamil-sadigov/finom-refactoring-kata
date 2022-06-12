namespace ReportService.Application.Report;

public readonly struct FileName
{
    public FileName(string value)
    {
        value.ThrowIfNull();

        if (!Path.HasExtension(value))
            throw new ArgumentException("File name should have extension", nameof(value));

        Value = value;
    }

    public string Value { get; }
    
    public static implicit operator string(FileName fileLocation) => fileLocation.Value;
    public static implicit operator FileName(string fileLocation) => new(fileLocation);
}