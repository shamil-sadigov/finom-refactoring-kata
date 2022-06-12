using FluentAssertions;
using Moq;
using ReportService.Application;
using ReportService.Application.Report;
using ReportService.Application.Report.Abstractions;
using ReportService.Application.Resolvers.BuhCodeResolver;
using ReportService.Application.Resolvers.SalaryResolver;
using ReportService.Tests.ReportService.Helpers;
using Xunit.Abstractions;

namespace ReportService.Tests.ReportService;

public class ReportProviderTests:IDisposable
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly IReportGenerator _reportGenerator;
    private readonly string _reportsRootDirectory;

    public ReportProviderTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _reportsRootDirectory = Path.Combine(Directory.GetCurrentDirectory(), "reports");

        Directory.CreateDirectory(_reportsRootDirectory);
        
        _reportGenerator = new ReportGenerator(
            new ReportInfoProvider(_reportsRootDirectory), 
            new ReportWriter());
    }
    
    [Fact]
    public async Task Should_create_expected_report()
    {
        // Arrange
        var (employeeSalaryResolver, employeeBuhCodeResolver) = CreateMocks();
        
        var reportProvider = new ReportProvider(
            _reportGenerator, 
            new EmployeeModelTransformation(employeeBuhCodeResolver, employeeSalaryResolver),
            InMemoryRepository.WithCollection(EmployeeTestData.EmployeeDataModels));
        
        // Act
        var report = await reportProvider.CreateReportAsync(2020, 10, CancellationToken.None);
        
        // Assert
        var expectedReport = await GetExpectedReportAsync();
        var actualReport = await report.AsTextAsync();
        
        actualReport.Should().Be(expectedReport);
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

    public void Dispose()
    {
        if (Directory.Exists(_reportsRootDirectory)) 
            Directory.Delete(_reportsRootDirectory, true);
    }
    
    private static async Task<string> GetExpectedReportAsync()
    {
        var report = Path.Combine(Directory.GetCurrentDirectory(), "ReportService\\Expected_report.txt");
        var expectedReport = await File.ReadAllTextAsync(report);
        return expectedReport;
    }
}