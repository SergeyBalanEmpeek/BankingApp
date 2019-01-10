using System.Threading.Tasks;

namespace BankingApp.DataAccess
{
    public interface IUnitOfWork
    {
        AccountRepository Accounts { get; }
        TransactionsRepopository Transactions { get; }

        /// <summary>
        /// Saves all changes made to the database.
        /// </summary>
        Task SaveAsync();

        /// <summary>
        /// Saves all changes made to the database.
        /// </summary>
        void Save();
    }
}
