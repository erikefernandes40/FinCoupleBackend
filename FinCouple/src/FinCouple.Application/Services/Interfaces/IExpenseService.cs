using FinCouple.Application.DTOs.Expenses;

namespace FinCouple.Application.Services.Interfaces;

public interface IExpenseService
{
    Task<IEnumerable<ExpenseResponse>> GetByCoupleAsync(Guid coupleId, CancellationToken cancellationToken = default);
    Task<ExpenseResponse> CreateAsync(CreateExpenseRequest request, CancellationToken cancellationToken = default);
    Task<ExpenseResponse?> UpdateAsync(Guid id, UpdateExpenseRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
