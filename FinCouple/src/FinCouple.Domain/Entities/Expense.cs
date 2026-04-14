namespace FinCouple.Domain.Entities;

public class Expense
{
    public Guid Id { get; private set; }
    public Guid CoupleId { get; private set; }
    public Guid CategoryId { get; private set; }
    public Guid PaidByUserId { get; private set; }
    public Guid? RecurringExpenseId { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public DateTime Date { get; private set; }
    public bool IsRecurring { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Expense() { }

    public static Expense Create(Guid coupleId, Guid categoryId, Guid paidByUserId,
        string description, decimal amount, DateTime date, bool isRecurring = false,
        Guid? recurringExpenseId = null)
    {
        return new Expense
        {
            Id = Guid.NewGuid(),
            CoupleId = coupleId,
            CategoryId = categoryId,
            PaidByUserId = paidByUserId,
            RecurringExpenseId = recurringExpenseId,
            Description = description,
            Amount = amount,
            Date = date,
            IsRecurring = isRecurring,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(Guid categoryId, Guid paidByUserId, string description,
        decimal amount, DateTime date)
    {
        CategoryId = categoryId;
        PaidByUserId = paidByUserId;
        Description = description;
        Amount = amount;
        Date = date;
    }
}
