
using FluentAssertions;
using ReportService.Application.Report;

namespace ReportService.Tests;

public class ReportLocationProviderTests
{
    [Theory]
    [InlineData(2020, 09)]
    [InlineData(2018, 05)]
    [InlineData(2010, 12)]
    public void Should_return_expected_report_location(int year, int month)
    {
        // Arrange
        var locationProvider = new ReportLocationProvider();

        var expectedPath = Path.Combine(
            Directory.GetCurrentDirectory(), 
            @$"reports\2020\accounting-report-{year}-{month}.txt");
        
        // Act
        var filePath = locationProvider.GetReportLocation(year, month);
        
        // Assert
        filePath.Should().Be(expectedPath);
    }
    
    [Theory]
    [InlineData(-12, 0)]
    [InlineData(0, 05)]
    [InlineData(2010, 15)]
    [InlineData(2010, -3)]
    public void Cannot_get_report_location_when_provided_time_is_invalid(int year, int month)
    {
        var pathBuilder = new ReportLocationProvider();
        
        pathBuilder.Invoking(x => x.GetReportLocation(year, month))
            .Should()
            .Throw<ArgumentOutOfRangeException>();
    }
}