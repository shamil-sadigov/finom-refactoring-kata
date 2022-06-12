
using FluentAssertions;
using ReportService.Application.Report;

namespace ReportService.Tests;

public class ReportInfoProviderTests
{
    [Theory]
    [InlineData(2020, 09)]
    [InlineData(2018, 05)]
    [InlineData(2010, 12)]
    public void Should_return_expected_report_location(int year, int month)
    {
        // Arrange
        var reportInfoProvider = new ReportInfoProvider();

        var expectedReportLocation = Path.Combine(
            Directory.GetCurrentDirectory(), 
            @$"reports\{year}\accounting-report-{year}-{month}.txt");
        
        // Act
        ReportInfo reportInfo = reportInfoProvider.GetReportInfo(year, month);
        
        // Assert
        reportInfo.Location.Should().Be(expectedReportLocation);
    }
    
    [Theory]
    [InlineData(-12, 0)]
    [InlineData(0, 05)]
    [InlineData(2010, 15)]
    [InlineData(2010, -3)]
    public void Cannot_get_report_info_when_provided_time_is_invalid(int year, int month)
    {
        var reportInfoProvider = new ReportInfoProvider();
        
        reportInfoProvider.Invoking(x => x.GetReportInfo(year, month))
            .Should()
            .Throw<ArgumentOutOfRangeException>();
    }
}