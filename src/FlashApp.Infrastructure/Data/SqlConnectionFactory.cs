using FlashApp.Application.Abstractions.Data;
using Npgsql;
using System.Data;
using Microsoft.Data.SqlClient;

namespace FlashApp.Infrastructure.Data;
internal sealed class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory
{
    public IDbConnection CreateConnection()
    {
        var connection = new SqlConnection(connectionString);
        connection.Open();

        return connection;
    }
}
