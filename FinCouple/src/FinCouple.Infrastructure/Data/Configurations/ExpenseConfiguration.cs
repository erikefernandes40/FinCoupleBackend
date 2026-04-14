using FinCouple.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinCouple.Infrastructure.Data.Configurations;

public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Description).HasMaxLength(500);
        builder.Property(e => e.Amount).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(e => e.Date).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();
    }
}
