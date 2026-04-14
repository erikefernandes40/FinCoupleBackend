using FinCouple.Domain.Enums;

namespace FinCouple.Domain.Entities;

public class RecurringExpense
{
    public Guid Id { get; private set; }
    public Guid CoupleId { get; private set; }
    public Guid CategoryId { get; private set; }
    public Guid CreatedByUserId { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public RecurrenceType RecurrenceType { get; private set; }
    public int DayOfMonth { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime NextDueDate { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private RecurringExpense() { }

    public static RecurringExpense Create(Guid coupleId, Guid categoryId, Guid createdByUserId,
        string description, decimal amount, RecurrenceType recurrenceType, int dayOfMonth,
        DateTime nextDueDate)
    {
        return new RecurringExpense
        {
            Id = Guid.NewGuid(),
            CoupleId = coupleId,
            CategoryId = categoryId,
            CreatedByUserId = createdByUserId,
            Description = description,
            Amount = amount,
            RecurrenceType = recurrenceType,
            DayOfMonth = dayOfMonth,
            IsActive = true,
            NextDueDate = nextDueDate,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void ToggleActive() => IsActive = !IsActive;

    public void AdvanceNextDueDate()
    {
        NextDueDate = RecurrenceType switch
        {
            RecurrenceType.Monthly => NextDueDate.AddMonths(1),
            RecurrenceType.Weekly => NextDueDate.AddDays(7),
            RecurrenceType.Yearly => NextDueDate.AddYears(1),
            _ => NextDueDate.AddMonths(1)
        };
    }
}
