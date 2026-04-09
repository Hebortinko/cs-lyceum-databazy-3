using DatabazyApiStarter.Models;
using DatabazyApiStarter.Repositories;

namespace DatabazyApiStarter.Services;

public class AuthService
{
    private readonly UserRepository _userRepository;

    public AuthService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return CreateError("Email je povinny.");
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            return CreateError("Heslo je povinne.");
        }

        if (!request.Email.Contains('@'))
        {
            return CreateError("Email nema spravny format.");
        }

        var user = await _userRepository.GetByEmailAsync(request.Email.Trim());
        if (user is null)
        {
            return CreateError("Pouzivatel neexistuje.");
        }

        if (!user.IsActive)
        {
            return CreateError("Pouzivatel je neaktivny.");
        }

        // Starter demo: plain text compare. In real apps passwords must be hashed.
        if (user.Password != request.Password)
        {
            return CreateError("Nespravne heslo.");
        }

        return new LoginResponse
        {
            Success = true,
            Message = "Prihlasenie uspesne.",
            DisplayName = user.Name,
            Email = user.Email,
            Role = user.Role
        };
    }

    private static LoginResponse CreateError(string message)
    {
        return new LoginResponse
        {
            Success = false,
            Message = message
        };
    }
}
