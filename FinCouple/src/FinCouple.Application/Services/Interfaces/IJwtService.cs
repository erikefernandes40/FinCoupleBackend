using FinCouple.Domain.Entities;

namespace FinCouple.Application.Services.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}
