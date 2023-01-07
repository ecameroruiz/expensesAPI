using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Entities;
using Application.Dtos;
using Application.Interfaces;
using Domain.Enums;
using Infrastructure.Interfaces;
using Application.Enums;

namespace Application.Services
{
    public class ExpenseCommandService : IExpenseCommandService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IUserRepository _userRepository;

        private readonly IExpenseCommandConverter _expenseCommandConverter;
        private readonly IExpenseQueryConverter _expenseQueryConverter;

        public ExpenseCommandService(
            IExpenseRepository expenseRepository,
            IUserRepository userRepository,
            IExpenseCommandConverter expenseCommandConverter,
            IExpenseQueryConverter expenseQueryConverter)
        {
            _expenseRepository = expenseRepository;
            _userRepository = userRepository;
            _expenseCommandConverter = expenseCommandConverter;
            _expenseQueryConverter = expenseQueryConverter;
        }

        public async Task<Result> CreateExpense(ExpenseCommandDto expenseCommandDto)
        {
            // Date cannot be in the future
            if (expenseCommandDto.Date > DateTime.UtcNow)
            {
                return new Result(ResultType.BadRequest, "Expense's date cannot be in the future");
            }

            // Date cannot be older than 3 months
            if (expenseCommandDto.Date < DateTime.UtcNow.AddMonths(-3))
            {
                return new Result(ResultType.BadRequest, "Expense's date cannot be older than 3 months");
            }

            // Cannot have empty comment
            if (string.IsNullOrWhiteSpace(expenseCommandDto.Comment))
            {
                return new Result(ResultType.BadRequest, "Expense's comment cannot be empty");
            }

            // Check expense type is valid
            if (!Enum.TryParse(expenseCommandDto.ExpenseType, out ExpenseType expenseType))
            {
                return new Result(ResultType.BadRequest, $"Expense's type {expenseCommandDto.ExpenseType} doesn't match any existing type: [{string.Join(",", Enum.GetNames(typeof(ExpenseType)))}]");
            }

            // Check currency is valid
            if (!Enum.TryParse(expenseCommandDto.Currency, out Currency currency))
            {
                return new Result(ResultType.BadRequest, $"Expense's currency {expenseCommandDto.Currency} doesn't match any existing currency: [{string.Join(",", Enum.GetNames(typeof(Currency)))}]");
            }

            // Get associated user 
            var user = await _userRepository.GetUserById(expenseCommandDto.UserId);

            // Check provided user exists
            if (user == null)
            {
                return new Result(ResultType.NotFound, $"No user found with id {expenseCommandDto.UserId}");
            }

            // Check new currency mathes user's current currency
            if (currency != user.Currency)
            {
                return new Result(ResultType.BadRequest, $"Expense's currency {currency} doesn't match user's currency {user.Currency}");
            }

            // Get associated user's existing expenses
            var expenses = await _expenseRepository.GetExpensesByUserId(expenseCommandDto.UserId);

            // Check there are no existing expenses with same date and amount for this user
            if (expenses.Any(expense => expense.Amount == expenseCommandDto.Amount && expense.Date == expenseCommandDto.Date))
            {
                return new Result(ResultType.Duplicated, $"Expense already exists for user with id {expenseCommandDto.UserId} with amount {expenseCommandDto.Amount} and date {expenseCommandDto.Date.ToShortDateString()}");
            }

            // Convert to expense entity
            var expense = _expenseCommandConverter.FromCommandDto(expenseCommandDto);

            // Create and save expense on DB
            var createdExpense = await _expenseRepository.CreateExpense(expense);

            // Convert to query dto and return created expense
            var expenseQueryDto = _expenseQueryConverter.ToQueryDto(createdExpense, user);
            return new Result(ResultType.Created, expenseQueryDto) { Location = $"expense/{createdExpense.Id}"};
        }
    }
}
