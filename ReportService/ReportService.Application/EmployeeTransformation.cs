using ReportService.Application.Resolvers.Abstractions;

namespace ReportService.Application;

/// <summary>
/// Responsible for transforming employee to reportable items based on which reports can be generated
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
    
    public Task<EmployeeReportableModel[]> TransformToReportableModelsAsync(
        IReadOnlyCollection<EmployeeDataModel> employees,
        CancellationToken cancellationToken)
    {
        List<Task<EmployeeReportableModel>> employeeTransformationTasks = new(employees.Count);
            
        foreach (var employee in employees)
        {
            var transformationTask = TransformToReportableModelAsync(employee, cancellationToken);
            employeeTransformationTasks.Add(transformationTask);
        }

        return Task.WhenAll(employeeTransformationTasks);
    }

    private Task<EmployeeReportableModel> TransformToReportableModelAsync(
        EmployeeDataModel employee, 
        CancellationToken cancellationToken)
    {
        var convertToReportableItemTask = _employeeBuhCodeResolver
            .GetEmployeeBuhcodeAsync(employee.Inn, cancellationToken)
            .ContinueWith(async buhCodeResolverTask =>
            {
                if (!buhCodeResolverTask.IsCompletedSuccessfully)
                {
                    throw new InvalidOperationException(
                        $"Something went wrong during getting employee buh code by Inn : {employee.Inn}",
                        buhCodeResolverTask.Exception);
                }

                var employeeBuhCode = buhCodeResolverTask.Result;

                // Странно что при получении зарплаты мы не используем значения year
                // или month которые нам приходят в контроллер
                // А откуда же тогда удаленный сервис знает за какой период зарплату мы хотим получить ?
                // Это надо обусудить
                
                var employeeSalary = 
                    await _salaryResolver.GetSalaryAsync(employeeBuhCode, employee.Inn, cancellationToken);

                return new EmployeeReportableModel(employee.Name, employee.Inn, employee.Department, employeeSalary);
            }, cancellationToken).Unwrap();
            
        return convertToReportableItemTask;
    }

}