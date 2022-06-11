namespace ReportService.Services.Report;

public class ReportLocation
{
    // TODO: Add validation
    public ReportLocation(ReportLocationType locatedAt, string locationAddress)
    {
        LocatedAt = locatedAt;
        LocationAddress = locationAddress;
    }
    
    public ReportLocationType LocatedAt { get; }
    public string LocationAddress { get; }
}