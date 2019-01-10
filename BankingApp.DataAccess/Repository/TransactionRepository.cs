using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.DataAccess
{
    public class TransactionsRepopository : GenericRepository<Transaction>
    {
        private readonly DbContext _dbContext;
        public TransactionsRepopository(DbContext dbContext): base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// This constructor is for Unit Testing purposes only
        /// </summary>
        public TransactionsRepopository() : base(null)
        {

        }

        /// <summary>
        /// Get list of transactions for specified account
        /// </summary>
        /// <param name="accountId">account to get transactions</param>
        /// <returns></returns>
        public async virtual Task<IEnumerable<Transaction>> GetTransactions(int accountId)
        {
            return await _dbContext.Set<Transaction>()
                .Include(c => c.Receiver)
                .Include(c => c.Sender)
                .Where(c => c.Sender.Id == accountId || c.Receiver.Id == accountId)
                .ToListAsync();
        }
    }
}
