using Domain.Enums;

namespace Domain.Entities
{
    public class User
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }        

        public Currency Currency { get; set; }
    }
}