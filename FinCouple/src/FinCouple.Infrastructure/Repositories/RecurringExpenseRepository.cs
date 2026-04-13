using FinCouple.Domain.Entities;
using FinCouple.Domain.Repositories;
using FinCouple.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinCouple.Infrastructure.Repositories;

public class RecurringExpenseRepository : BaseRepository<RecurringExpense>, IRecurringExpenseRepository
{
    public RecurringExpenseRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<RecurringExpense>> GetByCoupleAsync(Guid coupleId, CancellationToken cancellationToken = default)
        => await _context.RecurringExpenses.Where(r => r.CoupleId == coupleId).ToListAsync(cancellationToken);

    public async Task<IEnumerable<RecurringExpense>> GetActiveDueAsync(DateTime date, CancellationToken cancellationToken = default)
        => await _context.RecurringExpenses
            .Where(r => r.IsActive && r.NextDueDate.Date <= date.Date)
            .ToListAsync(cancellationToken);
}
