using FinCouple.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinCouple.Infrastructure.Data.Configurations;

public class RecurringExpenseConfiguration : IEntityTypeConfiguration<RecurringExpense>
{
    public void Configure(EntityTypeBuilder<RecurringExpense> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Description).HasMaxLength(500);
        builder.Property(r => r.Amount).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(r => r.RecurrenceType).HasConversion<string>();
        builder.Property(r => r.CreatedAt).IsRequired();
    }
}
