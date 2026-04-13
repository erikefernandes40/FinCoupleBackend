using FinCouple.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinCouple.Infrastructure.Data.Configurations;

public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.LimitAmount).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(b => b.Month).IsRequired();
        builder.Property(b => b.Year).IsRequired();
        builder.HasIndex(b => new { b.CoupleId, b.CategoryId, b.Month, b.Year }).IsUnique();
    }
}
