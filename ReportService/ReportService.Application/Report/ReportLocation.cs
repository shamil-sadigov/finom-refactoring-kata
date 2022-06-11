namespace ReportService.Application.Report;

public class ReportLocation
{
    // TODO: Add validation
    public ReportLocation(ReportLocationType locationType, string locationAddress)
    {
        LocationType = locationType;
        LocationAddress = locationAddress;
    }
    
    public ReportLocationType LocationType { get; }
    public string LocationAddress { get; }
}