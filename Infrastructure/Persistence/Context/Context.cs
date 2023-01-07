using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence.Configurations;
using Infrastructure.Dtos;

namespace Infrastructure.Persistence.Context
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }
        
        public DbSet<ExpenseDbDto> Expenses { get; set; }
        public DbSet<UserDbDto> Users { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DbExpenseConfiguration());
            modelBuilder.ApplyConfiguration(new DbUserConfiguration());
        }
    }
}