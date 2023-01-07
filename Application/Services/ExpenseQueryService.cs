using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Entities;
using Application.Dtos;
using Application.Interfaces;
using Application.Enums;
using Infrastructure.Interfaces;

namespace Application.Services
{
    public class ExpenseQueryService : IExpenseQueryService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IUserRepository _userRepository;

        private readonly IExpenseQueryConverter _expenseQueryConverter;

        public ExpenseQueryService(
            IExpenseRepository expenseRepository,
            IUserRepository userRepository,
            IExpenseQueryConverter expenseQueryConverter)
        {
            _expenseRepository = expenseRepository;
            _userRepository = userRepository;
            _expenseQueryConverter = expenseQueryConverter;
        }

        public async Task<Result> GetExpensesByUser(long userId, bool orderByAmount, bool orderByDate)
        {
            // Check query params
            if (orderByAmount && orderByDate) return new Result(ResultType.BadRequest, $"Cannot sort by amount and date at the same time");

            // Get user
            var user = await _userRepository.GetUserById(userId);

            // Check user exists
            if (user == null)
            {
                return new Result(ResultType.NotFound, $"No user found with id {userId}");
            }

            // Get all expenses matching user id
            var expenses = await _expenseRepository.GetExpensesByUserId(userId);

            // If no expenses exist for provided user, just return empty list
            if (!expenses.Any()) return new Result(ResultType.Ok, new List<ExpenseQueryDto>());

            // Apply amount sorting if set
            if (orderByAmount) 
            {
                expenses = expenses.OrderBy(expense => expense.Amount); 
            }

            // Apply date sorting if set
            if (orderByDate)
            {
                expenses = expenses.OrderBy(expense => expense.Date);
            }

            // Convert to query dtos and return list of expenses
            var expensesDtos = expenses.Select(expense => _expenseQueryConverter.ToQueryDto(expense, user));
            return new Result(ResultType.Ok, expensesDtos);
        }
    }
}
