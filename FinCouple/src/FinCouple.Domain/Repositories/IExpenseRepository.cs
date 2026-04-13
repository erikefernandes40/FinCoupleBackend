using FinCouple.Domain.Entities;

namespace FinCouple.Domain.Repositories;

public interface IExpenseRepository : IRepository<Expense>
{
    Task<IEnumerable<Expense>> GetByCoupleAsync(Guid coupleId, CancellationToken cancellationToken = default);
}
