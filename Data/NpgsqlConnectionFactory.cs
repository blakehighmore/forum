using System.Data;
using Npgsql;


namespace backend.Data;

public class NpgsqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public NpgsqlConnectionFactory(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection")!;
    }

    public IDbConnection Create() => new NpgsqlConnection(_connectionString);
}