﻿using ReportService.Application.BuhCodeResolver;
using ReportService.Application.SalaryProvider;

namespace ReportService.Application;

// TODO: Add comments

public sealed class EmployeeModelTransformation
{
    private readonly IEmployeeCodeResolver _employeeCodeResolver;
    private readonly IEmployeeSalaryProvider _salaryProvider;

    public EmployeeModelTransformation(
        IEmployeeCodeResolver employeeCodeResolver, 
        IEmployeeSalaryProvider salaryProvider)
    {
        _employeeCodeResolver = employeeCodeResolver;
        _salaryProvider = salaryProvider;
    }
        
    public Task<EmployeeReportItem[]> TransformToReportableItemsAsync(
        IReadOnlyCollection<EmployeeModel> employees,
        CancellationToken cancellationToken)
    {
        List<Task<EmployeeReportItem>> employeeTransformationTasks = new(employees.Count);
            
        foreach (var employeeDataModel in employees)
        {
            var transformationTask = TransformToReportableItemAsync(employeeDataModel, cancellationToken);
            employeeTransformationTasks.Add(transformationTask);
        }

        return Task.WhenAll(employeeTransformationTasks);
    }

    private Task<EmployeeReportItem> TransformToReportableItemAsync(
        EmployeeModel employee, 
        CancellationToken cancellationToken)
    {
        var convertToReportableItemTask = _employeeCodeResolver
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
                    await _salaryProvider.GetSalaryAsync(employeeBuhCode, employee.Inn, cancellationToken);

                return new EmployeeReportItem(employee.Name, employee.Inn, employee.Department, employeeSalary);
            }, cancellationToken).Unwrap();
            
        return convertToReportableItemTask;
    }

}