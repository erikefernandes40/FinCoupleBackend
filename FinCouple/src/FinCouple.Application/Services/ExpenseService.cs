using FinCouple.Application.DTOs.Expenses;
using FinCouple.Application.Services.Interfaces;
using FinCouple.Domain.Entities;
using FinCouple.Domain.Repositories;

namespace FinCouple.Application.Services;

public class ExpenseService : IExpenseService
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IEventPublisher _eventPublisher;

    public ExpenseService(IExpenseRepository expenseRepository, IEventPublisher eventPublisher)
    {
        _expenseRepository = expenseRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<IEnumerable<ExpenseResponse>> GetByCoupleAsync(Guid coupleId, CancellationToken cancellationToken = default)
    {
        var expenses = await _expenseRepository.GetByCoupleAsync(coupleId, cancellationToken);
        return expenses.Select(MapToResponse);
    }

    public async Task<ExpenseResponse> CreateAsync(CreateExpenseRequest request, CancellationToken cancellationToken = default)
    {
        var expense = Expense.Create(
            request.CoupleId, request.CategoryId, request.PaidByUserId,
            request.Description, request.Amount, request.Date,
            request.IsRecurring, request.RecurringExpenseId);

        await _expenseRepository.AddAsync(expense, cancellationToken);

        var response = MapToResponse(expense);
        await _eventPublisher.PublishExpenseCreatedAsync(expense.CoupleId, response, cancellationToken);

        return response;
    }

    public async Task<ExpenseResponse?> UpdateAsync(Guid id, UpdateExpenseRequest request, CancellationToken cancellationToken = default)
    {
        var expense = await _expenseRepository.GetByIdAsync(id, cancellationToken);
        if (expense is null) return null;

        expense.Update(request.CategoryId, request.PaidByUserId, request.Description, request.Amount, request.Date);
        await _expenseRepository.UpdateAsync(expense, cancellationToken);

        return MapToResponse(expense);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var expense = await _expenseRepository.GetByIdAsync(id, cancellationToken);
        if (expense is null) return false;

        await _expenseRepository.DeleteAsync(expense, cancellationToken);
        return true;
    }

    private static ExpenseResponse MapToResponse(Expense expense) => new()
    {
        Id = expense.Id,
        CoupleId = expense.CoupleId,
        CategoryId = expense.CategoryId,
        PaidByUserId = expense.PaidByUserId,
        RecurringExpenseId = expense.RecurringExpenseId,
        Description = expense.Description,
        Amount = expense.Amount,
        Date = expense.Date,
        IsRecurring = expense.IsRecurring,
        CreatedAt = expense.CreatedAt
    };
}
