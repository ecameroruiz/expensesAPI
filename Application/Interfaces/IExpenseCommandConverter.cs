using Application.Dtos;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IExpenseCommandConverter
    {
        Expense FromCommandDto(ExpenseCommandDto expenseCommandDto);
    }
}
