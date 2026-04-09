using Npgsql;

namespace DatabazyApiStarter;

public class Database
{
    private readonly string _connectionString;

    public Database()
    {
        _connectionString = BuildConnectionString();
    }

    public NpgsqlConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        return connection;
    }

    private static string BuildConnectionString()
    {
        var host = GetRequiredVariable("DB_HOST");
        var port = GetRequiredVariable("DB_PORT");
        var database = GetRequiredVariable("DB_NAME");
        var user = GetRequiredVariable("DB_USER");
        var password = GetRequiredVariable("DB_PASSWORD");

        return $"Host={host};" +
               $"Port={port};" +
               $"Database={database};" +
               $"Username={user};" +
               $"Password={password};";
    }

    private static string GetRequiredVariable(string variableName)
    {
        var value = Environment.GetEnvironmentVariable(variableName);

        if (value is null)
        {
            throw new InvalidOperationException(
                $"Environment variable '{variableName}' is missing. Check your .env file."
            );
        }

        return value;
    }
}
