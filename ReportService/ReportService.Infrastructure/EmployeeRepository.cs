using Dapper;
using ReportService.Application;

namespace ReportService.Infrastructure;

public class EmployeeRepository:IEmployeeRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public EmployeeRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    // Можно конечно результат запроса кешировать, но не известно о том как часто обновляется таблица
    // может быть несогласованность данных
    // и как следствие не известно какую стретегию ивалидации выбрать
    // Оставим как есть.
    
    public async Task<IReadOnlyList<EmployeeModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        var dbConnection = await _dbConnectionFactory.GetOrCreateConnection();
        
        var employees = await dbConnection.QueryAsync<EmployeeModel>(
            @"SELECT employees.name AS Name, 
                     employees.inn AS Inn, 
                     departments.name AS Department
              FROM employees
              INNER JOIN departments ON employees.departmentid = departments.id
              WHERE departments.active = true");

        return employees.ToList();
    }
}