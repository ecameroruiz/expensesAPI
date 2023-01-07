using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Dtos;

namespace Infrastructure.Converters
{
    public class UserDbConverter : IDbConverter<UserDbDto, User>
    {
        public UserDbDto ToDbDto(User user)
        {
            return new UserDbDto()
            {
                LastName = user.LastName,
                FirstName = user.FirstName,
                Currency = user.Currency,
            };
        }

        public User FromDbDto(UserDbDto userDbDto)
        {
            return new User()
            {
                Id = userDbDto.Id,
                LastName = userDbDto.LastName,
                FirstName = userDbDto.FirstName,
                Currency = userDbDto.Currency,
            };
        }
    }
}