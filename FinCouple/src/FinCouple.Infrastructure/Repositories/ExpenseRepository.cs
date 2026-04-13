using FinCouple.Domain.Entities;
using FinCouple.Domain.Repositories;
using FinCouple.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinCouple.Infrastructure.Repositories;

public class ExpenseRepository : BaseRepository<Expense>, IExpenseRepository
{
    public ExpenseRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Expense>> GetByCoupleAsync(Guid coupleId, CancellationToken cancellationToken = default)
        => await _context.Expenses.Where(e => e.CoupleId == coupleId).ToListAsync(cancellationToken);
}
