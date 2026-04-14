namespace FinCouple.Application.DTOs.Expenses;

public class ExpenseResponse
{
    public Guid Id { get; set; }
    public Guid CoupleId { get; set; }
    public Guid CategoryId { get; set; }
    public Guid PaidByUserId { get; set; }
    public Guid? RecurringExpenseId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public bool IsRecurring { get; set; }
    public DateTime CreatedAt { get; set; }
}
