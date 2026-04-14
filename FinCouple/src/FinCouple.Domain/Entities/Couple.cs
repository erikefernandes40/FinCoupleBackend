namespace FinCouple.Domain.Entities;

public class Couple
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    private Couple() { }

    public static Couple Create(string name)
    {
        return new Couple
        {
            Id = Guid.NewGuid(),
            Name = name,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateName(string name) => Name = name;
}
