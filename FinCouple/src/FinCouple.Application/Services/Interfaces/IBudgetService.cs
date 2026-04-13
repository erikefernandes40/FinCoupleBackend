using FinCouple.Application.DTOs.Budgets;

namespace FinCouple.Application.Services.Interfaces;

public interface IBudgetService
{
    Task<IEnumerable<BudgetResponse>> GetByCoupleAndMonthAsync(Guid coupleId, int month, int year, CancellationToken cancellationToken = default);
    Task<BudgetResponse> UpsertAsync(UpsertBudgetRequest request, CancellationToken cancellationToken = default);
}
