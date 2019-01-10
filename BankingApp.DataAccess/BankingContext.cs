using Microsoft.EntityFrameworkCore;

namespace BankingApp.DataAccess
{
    /// <summary>
    /// BankingApp database context
    /// </summary>
    public class BankingContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public BankingContext()
        {
            Database.EnsureCreated();
        }

        public BankingContext(DbContextOptions<BankingContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
