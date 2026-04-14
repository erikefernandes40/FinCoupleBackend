using FinCouple.Application.DTOs.Auth;
using FinCouple.Application.Services.Interfaces;
using FinCouple.Domain.Entities;
using FinCouple.Domain.Repositories;

namespace FinCouple.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public AuthService(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existing is not null)
            throw new InvalidOperationException("Email already in use.");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = User.Create(request.Name, request.Email, passwordHash);
        await _userRepository.AddAsync(user, cancellationToken);

        var token = _jwtService.GenerateToken(user);
        return new AuthResponse { Token = token, UserId = user.Id, Name = user.Name, Email = user.Email, CoupleId = user.CoupleId };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials.");

        var token = _jwtService.GenerateToken(user);
        return new AuthResponse { Token = token, UserId = user.Id, Name = user.Name, Email = user.Email, CoupleId = user.CoupleId };
    }
}
