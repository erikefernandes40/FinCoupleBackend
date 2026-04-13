using FinCouple.Domain.Entities;

namespace FinCouple.Domain.Repositories;

public interface IRecurringExpenseRepository : IRepository<RecurringExpense>
{
    Task<IEnumerable<RecurringExpense>> GetByCoupleAsync(Guid coupleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<RecurringExpense>> GetActiveDueAsync(DateTime date, CancellationToken cancellationToken = default);
}
