using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Infrastructure.Dtos
{
    public class UserDbDto
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public Currency Currency { get; set; }

        // Navigation property
        [ForeignKey("UserId")]
        public ICollection<ExpenseDbDto> Expenses { get; }
    }
}