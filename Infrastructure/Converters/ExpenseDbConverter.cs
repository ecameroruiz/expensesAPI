using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Dtos;

namespace Infrastructure.Converters
{
    public class ExpenseDbConverter : IDbConverter<ExpenseDbDto, Expense>
    {
        public ExpenseDbDto ToDbDto(Expense expense)
        {
            return new ExpenseDbDto()
            {
                UserId = expense.UserId,
                Date = expense.Date,
                ExpenseType = expense.ExpenseType,
                Amount = expense.Amount,
                Currency = expense.Currency,
                Comment = expense.Comment,
            };
        }

        public Expense FromDbDto(ExpenseDbDto expenseDbDto)
        {
            return new Expense()
            {
                Id = expenseDbDto.Id,
                UserId = expenseDbDto.UserId,
                Date = expenseDbDto.Date,
                ExpenseType = expenseDbDto.ExpenseType,
                Amount = expenseDbDto.Amount,
                Currency = expenseDbDto.Currency,
                Comment = expenseDbDto.Comment,
            };
        }
    }
}

