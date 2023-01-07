using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserById(long id);
    }
}