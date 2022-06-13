using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using ReportService.Application;
using ReportService.Application.Report;
using ReportService.Application.Report.Abstractions;
using ReportService.Application.Resolvers.BuhCodeResolver;
using ReportService.Application.Resolvers.SalaryResolver;
using ReportService.Tests.ReportService.Helpers;

namespace ReportService.Tests.ReportsProvider;

public class ReportProviderTests:IDisposable
{
    private readonly string _reportsRootDirectory;

    public ReportProviderTests()
    {
        _reportsRootDirectory = Path.Combine(Directory.GetCurrentDirectory(), $"reports/{nameof(ReportProviderTests)}");

        Directory.CreateDirectory(_reportsRootDirectory);
    }
    
    [Fact]
    public async Task Should_create_expected_report()
    {
        // Arrange
        string expectedReport = await GetExpectedReportAsync();
        ReportProvider sut = CreateSut();
        
        // Act
        Report report = await sut.CreateReportAsync(2020, 10, CancellationToken.None);
        
        // Assert
        string actualReportFromText = await report.AsTextAsync();
        string actualReportFromStream;
        
        using (var reportStream = report.AsStream())
            actualReportFromStream = await new StreamReader(reportStream).ReadToEndAsync();
        
        actualReportFromText.Should().Be(expectedReport);
        actualReportFromStream.Should().Be(expectedReport);
    }
    
    [Theory]
    [InlineData(2021, 08, 10)]
    [InlineData(2020, 10, 20)]
    [InlineData(2018, 05, 30)]
    public async Task Should_return_previously_created_report(int year, int month, int reportRequestedTimes)
    {
        // Arrange
        var reportWriterSpy = new ReportWriterSpy(new ReportWriter());
        var sut = CreateSut(reportWriterSpy);

        // Act
        for (int i = 0; i < reportRequestedTimes; i++)
        {
            Report report = await sut.CreateReportAsync(year, month, CancellationToken.None);
        }
        
        // Assert
        reportWriterSpy.ReportGeneratedCount
            .Should()
            .Be(1, because: "We should not generate the same report more than once");
    }
    
    private ReportProvider CreateSut(IReportWriter? reportWriter = null)
    {
        var (employeeSalaryResolver, employeeBuhCodeResolver) = CreateMocks();
        
        return new ReportProvider(
            new EmployeeTransformation(employeeBuhCodeResolver, employeeSalaryResolver),
            InMemoryRepository.WithCollection(EmployeeTestData.EmployeeDataModels),
            new ReportInfoProvider(_reportsRootDirectory),
            reportWriter ?? new ReportWriter());
    }
     
    private static (IEmployeeSalaryResolver, IEmployeeBuhCodeResolver) CreateMocks()
    {
        var salaryResolverMock = new Mock<IEmployeeSalaryResolver>();
        var buhCodeResolverMock = new Mock<IEmployeeBuhCodeResolver>();
        
        salaryResolverMock
            .Setup(x => x.GetSalaryAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string _, string inn, CancellationToken _) => EmployeeTestData.GetSalaryByInn(inn));

        buhCodeResolverMock
            .Setup(x => x.GetEmployeeBuhcodeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string employeeInn, CancellationToken _) => employeeInn);
        
        return (salaryResolverMock.Object, buhCodeResolverMock.Object);
    }
    
    private static async Task<string> GetExpectedReportAsync()
    {
        var report = Path.Combine(Directory.GetCurrentDirectory(), "ReportsProvider\\ReportExample.txt");
        var expectedReport = await File.ReadAllTextAsync(report);
        return expectedReport;
    }
    
    public void Dispose()
    {
        if (Directory.Exists(_reportsRootDirectory)) 
            Directory.Delete(_reportsRootDirectory, true);
    }
}