using DatabazyApiStarter.Models;
using Npgsql;

namespace DatabazyApiStarter.Repositories;

public class UserRepository
{
    private readonly Database _database;

    public UserRepository(Database database)
    {
        _database = database;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        await using var connection = _database.CreateConnection();
        await using var command = new NpgsqlCommand(@"
            SELECT id, name, email, password, is_active, role
            FROM users
            WHERE email = @email
            LIMIT 1;
        ", connection);

        command.Parameters.AddWithValue("email", email);

        await using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
            return null;
        }

        return new User
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Email = reader.GetString(2),
            Password = reader.GetString(3),
            IsActive = reader.GetBoolean(4),
            Role = reader.GetString(5)
        };
    }

    public async Task<List<User>> GetAllAsync()
    {
        var users = new List<User>();

        await using var connection = _database.CreateConnection();
        await using var command = new NpgsqlCommand(@"
            SELECT id, name, email, password, is_active, role
            FROM users
            ORDER BY id;
        ", connection);

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            users.Add(new User
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                Password = reader.GetString(3),
                IsActive = reader.GetBoolean(4),
                Role = reader.GetString(5)
            });
        }

        return users;
    }
}
