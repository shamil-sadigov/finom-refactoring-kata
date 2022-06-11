using System.Data;
using Npgsql;

namespace ReportService.Infrastructure;

public class NgSqlConnectionFactory : IDbConnectionFactory, IDisposable, IAsyncDisposable
{
    private readonly string _connectionString;
    private NpgsqlConnection? _connection;

    // "Host=192.168.99.100;Username=postgres;Password=1;Database=employee"
    public NgSqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
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