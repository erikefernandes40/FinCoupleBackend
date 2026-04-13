using FinCouple.Application.DTOs.RecurringExpenses;
using FinCouple.Application.Services.Interfaces;
using FinCouple.Domain.Entities;
using FinCouple.Domain.Repositories;

namespace FinCouple.Application.Services;

public class RecurringExpenseService : IRecurringExpenseService
{
    private readonly IRecurringExpenseRepository _recurringExpenseRepository;
    private readonly IExpenseRepository _expenseRepository;
    private readonly IEventPublisher _eventPublisher;

    public RecurringExpenseService(
        IRecurringExpenseRepository recurringExpenseRepository,
        IExpenseRepository expenseRepository,
        IEventPublisher eventPublisher)
    {
        _recurringExpenseRepository = recurringExpenseRepository;
        _expenseRepository = expenseRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<IEnumerable<RecurringExpenseResponse>> GetByCoupleAsync(Guid coupleId, CancellationToken cancellationToken = default)
    {
        var items = await _recurringExpenseRepository.GetByCoupleAsync(coupleId, cancellationToken);
        return items.Select(MapToResponse);
    }

    public async Task<RecurringExpenseResponse> CreateAsync(CreateRecurringExpenseRequest request, CancellationToken cancellationToken = default)
    {
        var entity = RecurringExpense.Create(
            request.CoupleId, request.CategoryId, request.CreatedByUserId,
            request.Description, request.Amount, request.RecurrenceType,
            request.DayOfMonth, request.NextDueDate);

        await _recurringExpenseRepository.AddAsync(entity, cancellationToken);
        return MapToResponse(entity);
    }

    public async Task<RecurringExpenseResponse?> ToggleActiveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _recurringExpenseRepository.GetByIdAsync(id, cancellationToken);
        if (entity is null) return null;

        entity.ToggleActive();
        await _recurringExpenseRepository.UpdateAsync(entity, cancellationToken);
        return MapToResponse(entity);
    }

    public async Task ProcessDueRecurrencesAsync(CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;
        var due = await _recurringExpenseRepository.GetActiveDueAsync(today, cancellationToken);

        foreach (var recurring in due)
        {
            var expense = Expense.Create(
                recurring.CoupleId, recurring.CategoryId, recurring.CreatedByUserId,
                recurring.Description, recurring.Amount, today, true, recurring.Id);

            await _expenseRepository.AddAsync(expense, cancellationToken);
            await _eventPublisher.PublishExpenseCreatedAsync(expense.CoupleId, new
            {
                expense.Id,
                expense.CoupleId,
                expense.CategoryId,
                expense.PaidByUserId,
                expense.Description,
                expense.Amount,
                expense.Date,
                expense.IsRecurring,
                expense.CreatedAt
            }, cancellationToken);

            recurring.AdvanceNextDueDate();
            await _recurringExpenseRepository.UpdateAsync(recurring, cancellationToken);
        }
    }

    private static RecurringExpenseResponse MapToResponse(RecurringExpense entity) => new()
    {
        Id = entity.Id,
        CoupleId = entity.CoupleId,
        CategoryId = entity.CategoryId,
        CreatedByUserId = entity.CreatedByUserId,
        Description = entity.Description,
        Amount = entity.Amount,
        RecurrenceType = entity.RecurrenceType,
        DayOfMonth = entity.DayOfMonth,
        IsActive = entity.IsActive,
        NextDueDate = entity.NextDueDate,
        CreatedAt = entity.CreatedAt
    };
}
