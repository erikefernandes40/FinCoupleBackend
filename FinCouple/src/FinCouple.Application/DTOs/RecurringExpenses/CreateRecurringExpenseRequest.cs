using FinCouple.Domain.Enums;

namespace FinCouple.Application.DTOs.RecurringExpenses;

public class CreateRecurringExpenseRequest
{
    public Guid CoupleId { get; set; }
    public Guid CategoryId { get; set; }
    public Guid CreatedByUserId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public RecurrenceType RecurrenceType { get; set; }
    public int DayOfMonth { get; set; }
    public DateTime NextDueDate { get; set; }
}
