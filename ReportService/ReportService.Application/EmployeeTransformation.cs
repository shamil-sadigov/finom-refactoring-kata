using ReportService.Application.Resolvers.BuhCodeResolver;
using ReportService.Application.Resolvers.SalaryResolver;

namespace ReportService.Application;

/// <summary>
/// Responsible for transforming employee to reportable items
/// </summary>
public sealed class EmployeeTransformation
{
    private readonly IEmployeeBuhCodeResolver _employeeBuhCodeResolver;
    private readonly IEmployeeSalaryResolver _salaryResolver;

    public EmployeeTransformation(
        IEmployeeBuhCodeResolver employeeBuhCodeResolver, 
        IEmployeeSalaryResolver salaryResolver)
    {
        _employeeBuhCodeResolver = employeeBuhCodeResolver;
        _salaryResolver = salaryResolver;
    }
    
    public Task<EmployeeReportableModel[]> TransformToReportableItemsAsync(
        IReadOnlyCollection<EmployeeDataModel> employees,
        CancellationToken cancellationToken)
    {
        List<Task<EmployeeReportableModel>> employeeTransformationTasks = new(employees.Count);
            
        foreach (var employeeDataModel in employees)
        {
            var transformationTask = TransformToReportableItemAsync(employeeDataModel, cancellationToken);
            employeeTransformationTasks.Add(transformationTask);
        }

        return Task.WhenAll(employeeTransformationTasks);
    }

    private Task<EmployeeReportableModel> TransformToReportableItemAsync(
        EmployeeDataModel employeeData, 
        CancellationToken cancellationToken)
    {
        var convertToReportableItemTask = _employeeBuhCodeResolver
            .GetEmployeeBuhcodeAsync(employeeData.Inn, cancellationToken)
            .ContinueWith(async buhCodeResolverTask =>
            {
                if (!buhCodeResolverTask.IsCompletedSuccessfully)
                {
                    throw new InvalidOperationException(
                        $"Something went wrong during getting employee buh code by Inn : {employeeData.Inn}",
                        buhCodeResolverTask.Exception);
                }

                var employeeBuhCode = buhCodeResolverTask.Result;

                // Странно что при получении зарплаты мы не используем значения year
                // или month которые нам приходят в контроллер
                // А откуда же тогда удаленный сервис знает за какой период зарплату мы хотим получить ?
                // Это надо обусудить
                
                var employeeSalary =
                    await _salaryResolver.GetSalaryAsync(employeeBuhCode, employeeData.Inn, cancellationToken);

                return new EmployeeReportableModel(employeeData.Name, employeeData.Inn, employeeData.Department, employeeSalary);
            }, cancellationToken).Unwrap();
            
        return convertToReportableItemTask;
    }

}