using System.Data;
using System.Data.SqlClient;

namespace Patients.Application.Database;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default);
}

public class SqlServerExpressConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;
    public SqlServerExpressConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default)
    {
        var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(token);
        return connection;
    }
}
