using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Infrastructure.Dtos;

namespace Infrastructure.Persistence.Configurations
{
    public class DbExpenseConfiguration : IEntityTypeConfiguration<ExpenseDbDto>
    {
        public void Configure(EntityTypeBuilder<ExpenseDbDto> builder)
        {
            builder.ToTable("Expense", "ContainerDb");
            
            builder.Property(e => e.Amount).HasColumnType("decimal(18,4)");
        }
    }
}