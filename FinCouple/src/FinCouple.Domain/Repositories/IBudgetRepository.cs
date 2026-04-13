using FinCouple.Domain.Entities;

namespace FinCouple.Domain.Repositories;

public interface IBudgetRepository : IRepository<Budget>
{
    Task<IEnumerable<Budget>> GetByCoupleAndMonthAsync(Guid coupleId, int month, int year, CancellationToken cancellationToken = default);
}
