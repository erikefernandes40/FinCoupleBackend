namespace FinCouple.Domain.Entities;

public class Budget
{
    public Guid Id { get; private set; }
    public Guid CoupleId { get; private set; }
    public Guid CategoryId { get; private set; }
    public decimal LimitAmount { get; private set; }
    public int Month { get; private set; }
    public int Year { get; private set; }

    private Budget() { }

    public static Budget Create(Guid coupleId, Guid categoryId, decimal limitAmount, int month, int year)
    {
        return new Budget
        {
            Id = Guid.NewGuid(),
            CoupleId = coupleId,
            CategoryId = categoryId,
            LimitAmount = limitAmount,
            Month = month,
            Year = year
        };
    }

    public void UpdateLimit(decimal limitAmount) => LimitAmount = limitAmount;
}
