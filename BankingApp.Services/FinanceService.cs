using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BankingApp.DataAccess;
using BankingApp.Common;

namespace BankingApp.Services
{
    public class FinanceService
    {
        /// <summary>
        /// How many retry attempts we could do during updating date
        /// </summary>
        private const int maxRetryConcurrencyAttempts = 3;

        /// <summary>
        /// Database instance
        /// </summary>
        private readonly IUnitOfWork _db;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db">Database instance</param>
        public FinanceService(IUnitOfWork db)
        {
            _db = db;
        }

        /// <summary>
        /// This constructor is for Unit Testing purposes only
        /// </summary>
        public FinanceService()
        {
        }

        /// <summary>
        /// Get possible recipients for a provided account
        /// </summary>
        /// <param name="accountId">Account to provide recipients</param>
        /// <returns></returns>
        public async virtual Task<IEnumerable<Account>> GetRecipientsForAccount(int accountId)
        {
            return await _db.Accounts.GetAll(c => c.Id != accountId);
        }

        /// <summary>
        /// Get all transactions, related to this account
        /// </summary>
        /// <param name="accountId">Account to get transactions</param>
        /// <returns></returns>
        public async virtual Task<IEnumerable<Transaction>> GetTransactions(int accountId)
        {
            return await _db.Transactions.GetTransactions(accountId);
        }

        /// <summary>
        /// Has user enough money on his account
        /// </summary>
        /// <param name="account">account</param>
        /// <param name="operationAmount">desired operation</param>
        /// <returns></returns>
        public virtual bool IsBalancePositive(Account account, decimal operationAmount)
        {
            //user wants to withdraw money
            if ((account.Balance + operationAmount) >= 0) return true;  //Final amount is positive. So allow this operation. OperationAmount is negative value

            //balance is negative. Disallow
            return false;
        }


        /// <summary>
        /// Try to change balance.
        /// </summary>
        /// <param name="accountId">Account ID to change balance</param>
        /// <param name="amount">Amount to change</param>
        /// <returns></returns>
        public async virtual Task<BalanceChangeResult> TryChangeBalance(int accountId, decimal amount)
        {
            bool saveFailed;
            do
            {
                saveFailed = false;

                Account account = await _db.Accounts.Get(c => c.Id == accountId);
                if (account == null) return new BalanceChangeResult(BalanceChangeResultType.Error);

                if (IsBalancePositive(account, amount) == false) return new BalanceChangeResult(BalanceChangeResultType.NegativeBalance, account);

                //Make balance change 
                account.Balance += amount;

                //Store this action as a transaction
                Transaction transaction = new Transaction()
                {
                    Sender = account,
                    Receiver = account,
                    Date = DateTime.Now,
                    Amount = amount
                };
                _db.Transactions.Create(transaction);

                try
                {
                    //Save to database
                    await _db.SaveAsync();

                    //Success
                    return new BalanceChangeResult(BalanceChangeResultType.Success, account);
                }
                catch (DbUpdateConcurrencyException)
                {
                    //https://docs.microsoft.com/en-gb/ef/core/saving/concurrency
                    //https://docs.microsoft.com/en-gb/ef/ef6/saving/concurrency

                    //Someone changed user balance right now
                    saveFailed = true;

                    //Detach this entity. Entity Framework will reload it.
                    _db.Accounts.DetachEntity(account);

                    //Not works: (cached values uses wrong values)
                    // Update the values of the entity that failed        
                    //foreach (var entry in ex.Entries)
                    //    await entry.ReloadAsync();

                }
            } while (saveFailed);

            return new BalanceChangeResult(BalanceChangeResultType.Error);
        }

        /// <summary>
        /// Try to transfer money.
        /// </summary>
        /// <param name="senderId">Account ID to send money</param>
        /// <param name="receiverId">Account ID to receive money</param>
        /// <param name="amount">Amount to transfer</param>
        /// <returns></returns>
        public async virtual Task<BalanceChangeResult> TryTransferMoney(int senderId, int receiverId, decimal amount)
        {
            bool saveFailed;
            do
            {
                saveFailed = false;

                Account sender = await _db.Accounts.Get(c => c.Id == senderId);
                if (sender == null) return new BalanceChangeResult(BalanceChangeResultType.Error);

                Account receiver = await _db.Accounts.Get(c => c.Id == receiverId);
                if (receiver == null) return new BalanceChangeResult(BalanceChangeResultType.Error);

                if (IsBalancePositive(sender, amount * -1) == false) return new BalanceChangeResult(BalanceChangeResultType.NegativeBalance, sender);

                //Make balance change 
                sender.Balance -= amount;
                receiver.Balance += amount;

                //Store this action as a transaction
                Transaction transaction = new Transaction()
                {
                    Sender = sender,
                    Receiver = receiver,
                    Date = DateTime.Now,
                    Amount = amount
                };
                _db.Transactions.Create(transaction);

                try
                {
                    //Save to database
                    await _db.SaveAsync();

                    //Success
                    return new BalanceChangeResult(BalanceChangeResultType.Success, sender);
                }
                catch (DbUpdateConcurrencyException)
                {
                    //Someone changed user balance right now
                    saveFailed = true;

                    //Detach these entities. Entity Framework will reload them.
                    _db.Accounts.DetachEntity(sender);
                    _db.Accounts.DetachEntity(receiver);

                    //Not works: 1) cached values uses wrong values   2) only 1 failed entity, while we need to reset 2 values
                    // Update the values of the entity that failed   
                    //foreach (var entry in ex.Entries)
                    //    await entry.ReloadAsync();
                }
                    
            } while (saveFailed);

            return new BalanceChangeResult(BalanceChangeResultType.Error);
        }
    }
}
