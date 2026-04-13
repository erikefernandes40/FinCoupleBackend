namespace FinCouple.Domain.Entities;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Color { get; private set; } = string.Empty;
    public string Icon { get; private set; } = string.Empty;
    public bool IsDefault { get; private set; }

    private Category() { }

    public static Category Create(string name, string color, string icon, bool isDefault = false)
    {
        return new Category
        {
            Id = Guid.NewGuid(),
            Name = name,
            Color = color,
            Icon = icon,
            IsDefault = isDefault
        };
    }

    public void Update(string name, string color, string icon) 
    {
        Name = name;
        Color = color;
        Icon = icon;
    }
}
