using System;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.Dtos
{
    public class ExpenseCommandDto
    {
        /// <summary>
        /// The user associated to the expense to be created
        /// </summary>
        /// <example>1</example>
        [Required]
        public long UserId { get; set; }

        /// <summary>
        /// The date of the expense to be created
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// The type of the expense to be created
        /// </summary>
        /// <example>Hotel</example>
        [Required]
        [EnumDataType(typeof(ExpenseType))]
        public string ExpenseType { get; set; }

        /// <summary>
        /// The amount of the expense to be created
        /// </summary>
        /// <example>100</example>
        [Required]
        public decimal Amount { get; set; }

        /// <summary>
        /// The currency of the expense to be created (must match the user's)
        /// </summary>
        /// <example>USD</example>
        [Required]
        [EnumDataType(typeof(Currency))]
        public string Currency { get; set; }

        /// <summary>
        /// The comment for the expense to be created
        /// </summary>
        /// <example>Expense comment</example>
        [Required]
        public string Comment { get; set; }
    }
}