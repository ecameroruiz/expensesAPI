using System.Threading.Tasks;
using Application.Entities;

namespace Application.Interfaces
{
    public interface IExpenseQueryService
    {
        Task<Result> GetExpensesByUser(long userId, bool orderByAmount, bool orderByDate);
    }
}
