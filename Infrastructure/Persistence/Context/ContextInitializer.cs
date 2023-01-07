using Infrastructure.Dtos;
using Domain.Enums;
using System.Linq;
using System;

namespace Infrastructure.Persistence.Context
{
    public static class ContextInitializer
    {
        public static void Initialize(Context context)
        {
            context.Database.EnsureCreated();
            
            if (!context.Users.Any())
            {
                InitializeUsers();
            }

            if (!context.Expenses.Any())
            {
                InitializeExpenses();
            }

            void InitializeUsers()
            {
                var defaultUsers = new UserDbDto[]
                {
                    new UserDbDto { LastName = "Stark", FirstName = "Anthony", Currency = Currency.USD },
                    new UserDbDto { LastName = "Romanova", FirstName = "Natasha", Currency = Currency.RUB }
                };

                context.Users.AddRange(defaultUsers);
                context.SaveChanges();
            }

            void InitializeExpenses()
            {
                var defaultExpenses = new ExpenseDbDto[]
                {
                    new ExpenseDbDto { UserId = 1, Comment = "Expense User #1", Date = DateTime.Now, Amount = 10, ExpenseType = ExpenseType.Restaurant, Currency = Currency.USD },
                    new ExpenseDbDto { UserId = 2, Comment = "Expense User #2", Date = DateTime.Now, Amount = 20, ExpenseType = ExpenseType.Hotel, Currency = Currency.RUB }
                };

                context.Expenses.AddRange(defaultExpenses);
                context.SaveChanges();
            }
        }
    }
}