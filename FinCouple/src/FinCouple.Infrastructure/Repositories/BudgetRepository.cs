using FinCouple.Domain.Entities;
using FinCouple.Domain.Repositories;
using FinCouple.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinCouple.Infrastructure.Repositories;

public class BudgetRepository : BaseRepository<Budget>, IBudgetRepository
{
    public BudgetRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Budget>> GetByCoupleAndMonthAsync(Guid coupleId, int month, int year, CancellationToken cancellationToken = default)
        => await _context.Budgets
            .Where(b => b.CoupleId == coupleId && b.Month == month && b.Year == year)
            .ToListAsync(cancellationToken);
}
