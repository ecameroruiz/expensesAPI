using Application.Entities;
using Application.Dtos;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IExpenseCommandService
    {
        Task<Result> CreateExpense(ExpenseCommandDto expenseCommandDto);
    }
}
