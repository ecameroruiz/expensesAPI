using Application.Dtos;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IExpenseQueryConverter
    {
        ExpenseQueryDto ToQueryDto(Expense expense, User user);
    }
}
