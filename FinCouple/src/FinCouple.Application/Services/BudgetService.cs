using FinCouple.Application.DTOs.Budgets;
using FinCouple.Application.Services.Interfaces;
using FinCouple.Domain.Entities;
using FinCouple.Domain.Repositories;

namespace FinCouple.Application.Services;

public class BudgetService : IBudgetService
{
    private readonly IBudgetRepository _budgetRepository;

    public BudgetService(IBudgetRepository budgetRepository)
    {
        _budgetRepository = budgetRepository;
    }

    public async Task<IEnumerable<BudgetResponse>> GetByCoupleAndMonthAsync(Guid coupleId, int month, int year, CancellationToken cancellationToken = default)
    {
        var budgets = await _budgetRepository.GetByCoupleAndMonthAsync(coupleId, month, year, cancellationToken);
        return budgets.Select(MapToResponse);
    }

    public async Task<BudgetResponse> UpsertAsync(UpsertBudgetRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await _budgetRepository.GetByCoupleAndMonthAsync(request.CoupleId, request.Month, request.Year, cancellationToken);
        var budget = existing.FirstOrDefault(b => b.CategoryId == request.CategoryId);

        if (budget is null)
        {
            budget = Budget.Create(request.CoupleId, request.CategoryId, request.LimitAmount, request.Month, request.Year);
            await _budgetRepository.AddAsync(budget, cancellationToken);
        }
        else
        {
            budget.UpdateLimit(request.LimitAmount);
            await _budgetRepository.UpdateAsync(budget, cancellationToken);
        }

        return MapToResponse(budget);
    }

    private static BudgetResponse MapToResponse(Budget budget) => new()
    {
        Id = budget.Id,
        CoupleId = budget.CoupleId,
        CategoryId = budget.CategoryId,
        LimitAmount = budget.LimitAmount,
        Month = budget.Month,
        Year = budget.Year
    };
}
