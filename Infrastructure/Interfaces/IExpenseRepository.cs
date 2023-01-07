using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Interfaces
{
    public interface IExpenseRepository
    {
        Task<Expense> CreateExpense(Expense expense);

        Task<IEnumerable<Expense>> GetExpensesByUserId(long userId);
    }
}