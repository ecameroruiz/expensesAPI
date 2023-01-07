using System;
using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Application.Converters
{
    public class ExpenseCommandConverter : IExpenseCommandConverter
    {
        public Expense FromCommandDto(ExpenseCommandDto expenseCommandDto)
        {
            return new Expense
            {
                UserId = expenseCommandDto.UserId,
                Amount = expenseCommandDto.Amount,
                Date = expenseCommandDto.Date,
                Comment = expenseCommandDto.Comment,              
                ExpenseType = (ExpenseType)Enum.Parse(typeof(ExpenseType), expenseCommandDto.ExpenseType),
                Currency = (Currency)Enum.Parse(typeof(Currency), expenseCommandDto.Currency),
            };
        }
    }
}
