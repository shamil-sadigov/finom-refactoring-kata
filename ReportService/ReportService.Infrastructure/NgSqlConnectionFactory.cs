using System.Data;
using Npgsql;
using ReportService.Application;

namespace ReportService.Infrastructure;

public class NgSqlConnectionFactory : IDbConnectionFactory, IDisposable, IAsyncDisposable
{
    private readonly string _connectionString;
    private NpgsqlConnection? _connection;
    
    public NgSqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString.ThrowIfNull();
    }
    
    public async Task<IDbConnection> GetOrCreateConnection()
    {
        if (_connection is {State: ConnectionState.Open})
            return _connection;
        
        _connection = new NpgsqlConnection(_connectionString);
        await _connection.OpenAsync();
        return _connection;
    }
    
    public void Dispose()
    {
        if (_connection is {State: ConnectionState.Open})
            _connection.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection is {State: ConnectionState.Open})
            await _connection.DisposeAsync();
    }
}