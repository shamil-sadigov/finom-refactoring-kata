using FluentAssertions;
using ReportService.Domain;
using ReportService.Services.Report;

namespace ReportService.Tests.ReportWriters;

public class ReportWriterTests
{
    [Fact]
    public async Task Should_write_expected_report()
    {
        // Arrange
        var employees = CreateSampleEmployees();
        var stringWriter = new StringWriter();
        var reportWriter = new ReportWriter();
        var expectedReport = await GetExpectedReport();

        // Act
        await reportWriter.WriteAsync(stringWriter, 2020, 10, employees);
        var actualReport = stringWriter.ToString();

        // Assert
        actualReport.Should().Be(expectedReport);
    }

    private static List<EmployeeReportItem> CreateSampleEmployees()
    {
        var employees = new List<EmployeeReportItem>
        {
            new("Ernest Hemingway", "1", "Писатели", 2000),

            new("John Fogerty", "3", "Музыканты", 5000),
            new("David Gilmour", "4", "Музыканты", 5000),

            new("Charles Babbage", "6", "IT", 6000),
            new("Ada Lovelace", "7", "IT", 6000),
            new("Tim Berners Lee", "5", "IT", 8000),
        };
        return employees;
    }

    private static async Task<string> GetExpectedReport()
    {
        var report = Path.Combine(Directory.GetCurrentDirectory(), "ReportWriters\\Expected_report.txt");

        var expectedReport = await File.ReadAllTextAsync(report);
        return expectedReport;
    }
}