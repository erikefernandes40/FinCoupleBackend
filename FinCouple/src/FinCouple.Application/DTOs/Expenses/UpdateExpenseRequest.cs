namespace FinCouple.Application.DTOs.Expenses;

public class UpdateExpenseRequest
{
    public Guid CategoryId { get; set; }
    public Guid PaidByUserId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}
