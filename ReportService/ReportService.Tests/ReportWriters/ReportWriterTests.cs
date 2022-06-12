using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using ReportService.Application;
using ReportService.Application.Report;

namespace ReportService.Tests.ReportWriters;

public class ReportWriterTests
{
    [Fact]
    public async Task Should_write_expected_report()
    {
        // Arrange
        var employees = CreateSampleEmployees();
        var expectedReport = await GetExpectedReport();
        
        var reportWriter = new ReportWriter();
        var stringWriter = new StringWriter();
        
        // Act
        await reportWriter.WriteAsync(stringWriter, 2020, 10, employees);
        var actualReport = stringWriter.ToString();

        // Assert
        actualReport.Should().Be(expectedReport);
    }

    private static List<EmployeeReportableModel> CreateSampleEmployees()
    {
        var employees = new List<EmployeeReportableModel>
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
        var report = Path.Combine(Directory.GetCurrentDirectory(), "ReportWriters\\ReportExample.txt");

        var expectedReport = await File.ReadAllTextAsync(report);
        return expectedReport;
    }
}