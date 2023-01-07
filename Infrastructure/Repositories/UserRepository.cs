using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Dtos;
using Infrastructure.Interfaces;
using Domain.Interfaces;
using Domain.Entities;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Context _context;

        private readonly IDbConverter<UserDbDto, User> _userDbConverter;

        public UserRepository(Context context, IDbConverter<UserDbDto, User> userDbConverter)
        {
            _context = context;
            _userDbConverter = userDbConverter;
        }

        public async Task<User?> GetUserById(long id)
        {
            var userDbDto = await _context.Users.SingleOrDefaultAsync(user => user.Id == id);
            return userDbDto != null ? _userDbConverter.FromDbDto(userDbDto) : null;
        }
    }
}