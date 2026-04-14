using FinCouple.Domain.Entities;
using FinCouple.Domain.Repositories;
using FinCouple.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinCouple.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
}
