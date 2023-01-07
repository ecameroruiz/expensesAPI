using Application.Converters;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Converters;
using Infrastructure.Dtos;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI.Extensions
{
    internal static class InjectionExtensions
    {
        internal static void AddConverters(this IServiceCollection services)
        {
            // add DB converters
            services.AddScoped<IDbConverter<ExpenseDbDto, Expense>, ExpenseDbConverter>();
            services.AddScoped<IDbConverter<UserDbDto, User>, UserDbConverter>();

            // add app converters
            services.AddScoped<IExpenseQueryConverter, ExpenseQueryConverter>();
            services.AddScoped<IExpenseCommandConverter, ExpenseCommandConverter>();
        }

        internal static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }

        internal static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IExpenseCommandService, ExpenseCommandService>();
            services.AddScoped<IExpenseQueryService, ExpenseQueryService>();
        }
    }
}
