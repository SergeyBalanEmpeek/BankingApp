using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingApp.DataAccess
{
    /// <summary>
    /// Account data
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Account identification number
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        ///  User Login
        /// </summary>
        [Required, Column(TypeName = "varchar(50)")]
        public string Login { get; set; }

        /// <summary>
        ///  Password Hash
        /// </summary>
        [Required, Column(TypeName = "varchar(44)")]
        public string PasswordHash { get; set; }

        /// <summary>
        ///  Password Salt
        /// </summary>
        [Required, Column(TypeName = "varchar(24)")]
        public string PasswordSalt { get; set; }

        /// <summary>
        /// Current balance
        /// </summary>
        [ConcurrencyCheck]             //verify field value in database transactions. throw an error is this value was changed
        [Column(TypeName = "money")]
        public decimal Balance { get; set; }
    }
}
