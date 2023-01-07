using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Dtos;
using Infrastructure.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly Context _context;

        private readonly IDbConverter<ExpenseDbDto, Expense> _expenseDbConverter;

        public ExpenseRepository(Context context, IDbConverter<ExpenseDbDto, Expense> expenseDbConverter)
        {
            _context = context;
            _expenseDbConverter = expenseDbConverter;
        }

        public async Task<Expense> CreateExpense(Expense expense)
        {
            var expenseDbDto = _expenseDbConverter.ToDbDto(expense);
            await _context.Expenses.AddAsync(expenseDbDto);
            await _context.SaveChangesAsync();
            return _expenseDbConverter.FromDbDto(expenseDbDto);
        }

        public async Task<IEnumerable<Expense>> GetExpensesByUserId(long userId)
        {
            var expensesDbDtos = await _context.Expenses.Where(expense => expense.UserId == userId).ToListAsync();
            return expensesDbDtos.Select(_expenseDbConverter.FromDbDto);
        }
    }
}