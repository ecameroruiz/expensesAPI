using System;

namespace Application.Dtos
{
    public class ExpenseQueryDto
    {
        /// <summary>
        /// The id of the expense
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The full name of the user associated to the expense
        /// </summary>
        public string UserFullName { get; set; }

        /// <summary>
        /// The date of the expense
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The type of the expense
        /// </summary>
        public string ExpenseType { get; set; }

        /// <summary>
        /// The expense amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// The expense currency type
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// The expense comment
        /// </summary>
        public string Comment { get; set; }
    }
}