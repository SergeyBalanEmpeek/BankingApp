using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingApp.DataAccess
{
    /// <summary>
    /// Transaction data
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Transaction identification number
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Transaction Date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Person, who sent money
        /// </summary>
        public Account Sender { get; set; }

        /// <summary>
        /// Person, who recieved money
        /// </summary>
        public Account Receiver { get; set; }

        /// <summary>
        /// Amount of money
        /// </summary>
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }
    }
}
