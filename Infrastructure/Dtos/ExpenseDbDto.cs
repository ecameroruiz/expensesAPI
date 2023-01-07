using System;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Infrastructure.Dtos
{
    public class ExpenseDbDto
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long UserId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public ExpenseType ExpenseType { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public Currency Currency { get; set; }

        [Required]
        public string Comment { get; set; }

        // Navigation property
        public UserDbDto User { get; }
    }
}