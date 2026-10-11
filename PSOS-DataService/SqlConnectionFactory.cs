using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace PSOS_DataService;

public class SqlConnectionFactory(IConfiguration config)
{
    private readonly string _connectionString = config.GetConnectionString("PsosDb")
        ?? throw new InvalidOperationException("Connection string 'PsosDb' is missing.");

    public SqlConnection Create() => new(_connectionString);

    public static SqlCommand CreateProcedureCommand(SqlConnection connection, string procedureName)
        => new(procedureName, connection) { CommandType = CommandType.StoredProcedure };
}