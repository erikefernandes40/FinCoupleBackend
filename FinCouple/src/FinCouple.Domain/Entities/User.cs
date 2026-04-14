namespace FinCouple.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public Guid? CoupleId { get; private set; }

    private User() { }

    public static User Create(string name, string email, string passwordHash)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email,
            PasswordHash = passwordHash
        };
    }

    public void SetCouple(Guid coupleId) => CoupleId = coupleId;
    public void UpdateName(string name) => Name = name;
}
