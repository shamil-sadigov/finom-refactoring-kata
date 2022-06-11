using System.Data;

namespace ReportService.Infrastructure;

// TODO: Ensure that dbConnection is disposed
// TODO: Register in DI COntainer
public interface IDbConnectionFactory
{
    Task<IDbConnection> GetOrCreateConnection();
}