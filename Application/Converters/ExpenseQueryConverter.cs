using System;
using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Application.Converters
{
    public class ExpenseQueryConverter : IExpenseQueryConverter
    {
        public ExpenseQueryDto ToQueryDto(Expense expense, User user)
        {
            return new ExpenseQueryDto
            {
                Id = expense.Id,
                Amount = expense.Amount,
                Comment = expense.Comment,
                Date = expense.Date,
                Currency = Enum.GetName(typeof(Currency), expense.Currency),
                ExpenseType = Enum.GetName(typeof(ExpenseType), expense.ExpenseType),
                UserFullName = $"{user.FirstName} {user.LastName}"
            };
        }
    }
}
