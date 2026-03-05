using Npgsql;

namespace ConsoleApp1;

public class Database
{
    private NpgsqlConnection _conn;

    public Database()
    {
        string connString = GetConnectionString();
        _conn = CreateDatabaseConnection(connString);
        OpenDatabaseConnection();
        Console.WriteLine("Pripojenie OK!");
    }

    private string GetConnectionString()
    {
        return $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
               $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
               $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
               $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
               $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};";
    }

    private NpgsqlConnection CreateDatabaseConnection(string connString)
    {
        return new NpgsqlConnection(connString);
    }

    private void OpenDatabaseConnection()
    {
        _conn.Open();
    }
    
    public NpgsqlDataReader Query(string sql)
    {
        var cmd = new NpgsqlCommand(sql, _conn);
        return cmd.ExecuteReader();
    }
    
    public void Execute(string sql)
    {
        var cmd = new NpgsqlCommand(sql, _conn);
        cmd.ExecuteNonQuery();
        Console.WriteLine("OK!");
    }


    public void Close()
    {
        _conn.Close();
    }
}