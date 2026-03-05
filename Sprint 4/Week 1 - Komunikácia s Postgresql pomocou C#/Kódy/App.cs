namespace ConsoleApp1;

public class App
{
    private Database _db;

    public App()
    {
        bool loaded = LoadEnv.LoadFile(".env");
        if (!loaded)
        {
            Console.WriteLine("Could not find .env file");
        }
        _db = new Database();
    }

    public void Query(string sql)
    {
        var reader = _db.Query(sql);

        while (reader.Read())
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write($"{reader.GetName(i)}: {reader[i]}  ");
            }
            Console.WriteLine();
        }
    }
    
    public void Execute(string sql)
    {
        _db.Execute(sql);
    }

    public void CreateTable(string tableName, Dictionary<string, string> columns)
    {
        var cols = string.Join(", ", columns.Select(c => $"{c.Key} {c.Value}"));
        Execute($"CREATE TABLE IF NOT EXISTS {tableName} ({cols})");
    }

    public void Insert(string tableName, Dictionary<string, string> data)
    {
        var cols = string.Join(", ", data.Keys);
        var vals = string.Join(", ", data.Values.Select(v => $"'{v}'"));
        Execute($"INSERT INTO {tableName} ({cols}) VALUES ({vals})");
    }

    public void Select(string tableName)
    {
        Query($"SELECT * FROM {tableName}");
    }

    public void Close()
    {
        _db.Close();
    }
}