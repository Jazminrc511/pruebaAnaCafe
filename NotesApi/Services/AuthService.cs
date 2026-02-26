using Microsoft.AspNetCore.Identity;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly ITokenService _tokenService;
    private readonly PasswordHasher<User> _hasher = new();

    public AuthService(IUserRepository userRepo, ITokenService tokenService)
    {
        _userRepo = userRepo;
        _tokenService = tokenService;
    }

    public async Task RegisterAsync(string email, string password)
    {
        var existing = await _userRepo.GetByEmailAsync(email);
        if (existing != null)
            throw new Exception("El correo ya está registrado");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            Role = "User"
        };

        user.PasswordHash = _hasher.HashPassword(user, password);

        await _userRepo.AddAsync(user);
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        var user = await _userRepo.GetByEmailAsync(email);
        if (user == null)
            throw new Exception("Credenciales inválidas");

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);

        if (result == PasswordVerificationResult.Failed)
            throw new Exception("Credenciales inválidas");

        return _tokenService.CreateToken(user);
    }
}