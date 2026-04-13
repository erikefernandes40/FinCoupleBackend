using FinCouple.Application.DTOs.RecurringExpenses;

namespace FinCouple.Application.Services.Interfaces;

public interface IRecurringExpenseService
{
    Task<IEnumerable<RecurringExpenseResponse>> GetByCoupleAsync(Guid coupleId, CancellationToken cancellationToken = default);
    Task<RecurringExpenseResponse> CreateAsync(CreateRecurringExpenseRequest request, CancellationToken cancellationToken = default);
    Task<RecurringExpenseResponse?> ToggleActiveAsync(Guid id, CancellationToken cancellationToken = default);
    Task ProcessDueRecurrencesAsync(CancellationToken cancellationToken = default);
}
