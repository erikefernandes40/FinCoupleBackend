namespace FinCouple.Application.DTOs.Budgets;

public class BudgetResponse
{
    public Guid Id { get; set; }
    public Guid CoupleId { get; set; }
    public Guid CategoryId { get; set; }
    public decimal LimitAmount { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
}
