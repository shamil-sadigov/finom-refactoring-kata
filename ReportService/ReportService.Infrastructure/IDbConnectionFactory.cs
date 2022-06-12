using System.Data;

namespace ReportService.Infrastructure;

public interface IDbConnectionFactory
{
    Task<IDbConnection> GetOrCreateConnection();
}