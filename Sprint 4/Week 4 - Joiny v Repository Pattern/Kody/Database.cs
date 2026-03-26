using Npgsql;

namespace EshopApp2;

public class Database
{
    private NpgsqlConnection _conn;

    public Database()
    {
        string connString = $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
                            $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                            $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
                            $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
                            $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};";

        _conn = new NpgsqlConnection(connString);
        _conn.Open();
        Console.WriteLine("Pripojenie OK!");
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
    }

    public void Close()
    {
        _conn.Close();
    }
}