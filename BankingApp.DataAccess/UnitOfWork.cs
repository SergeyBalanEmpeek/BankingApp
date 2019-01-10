using System.Threading.Tasks;

namespace BankingApp.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BankingContext _bankingContext;

        private AccountRepository accountsRepository;
        private TransactionsRepopository transactionsRepository;

        public UnitOfWork(BankingContext bankingContext)
        {
            _bankingContext = bankingContext;
        }

        /// <summary>
        /// Accounts
        /// </summary>
        public AccountRepository Accounts
        {
            get
            {
                return accountsRepository = accountsRepository ?? new AccountRepository(_bankingContext);
            }
        }

        /// <summary>
        /// Transactions
        /// </summary>
        public TransactionsRepopository Transactions
        {
            get
            {
                return transactionsRepository = transactionsRepository ?? new TransactionsRepopository(_bankingContext);
            }
        }

        /// <summary>
        /// Saves all changes made to the database.
        /// </summary>
        public virtual void Save()
        {
            _bankingContext.SaveChanges();
        }

        /// <summary>
        /// Saves all changes made to the database.
        /// </summary>
        public async virtual Task SaveAsync()
        {
            await _bankingContext.SaveChangesAsync();
        }
    }
}
