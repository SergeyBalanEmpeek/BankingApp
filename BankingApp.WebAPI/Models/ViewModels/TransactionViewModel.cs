using BankingApp.DataAccess;

namespace BankingApp
{
    public class TransactionViewModel
    {
        /// <summary>
        /// Transaction ID
        /// </summary>
        public int Id;

        /// <summary>
        /// Transaction Date
        /// </summary>
        public string Date;

        /// <summary>
        /// Money sender
        /// </summary>
        public string Sender;

        /// <summary>
        /// Money Receiver
        /// </summary>
        public string Receiver;

        /// <summary>
        /// Amount of money
        /// </summary>
        public decimal Amount;

        /// <summary>
        /// Detailed description to display
        /// </summary>
        public string Description;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="transaction">Source transacton info</param>
        /// <param name="accountId">Account to generate information</param>
        public TransactionViewModel(Transaction transaction, int accountId)
        {
            Id = transaction.Id;
            Date = transaction.Date.ToString("G");
            Sender = transaction.Sender.Login;
            Receiver = transaction.Receiver.Login;
            Amount = GenerateAmount(transaction, accountId);
            Description = GenerateDescription(transaction);
        }

        /// <summary>
        /// Generate detailed description for transaction
        /// </summary>
        /// <param name="transaction">transaction</param>
        /// <returns></returns>
        private string GenerateDescription(Transaction transaction)
        {
            if (transaction.Sender == transaction.Receiver)
            {
                if (transaction.Amount >= 0)
                    return $"{transaction.Sender.Login} deposited {transaction.Amount.ToString("0.00")}$";
                else
                    return $"{transaction.Sender.Login} withdrawn {(-1 * transaction.Amount).ToString("0.00")}$";
            }
            else
                return $"{transaction.Sender.Login} transferred {transaction.Amount.ToString("0.00")}$ to {transaction.Receiver.Login}'s account";
        }


        /// <summary>
        /// Correct amount value according to destination user to display
        /// </summary>
        /// <param name="transaction">transaction</param>
        /// <param name="accountId">related account</param>
        /// <returns></returns>
        private decimal GenerateAmount(Transaction transaction, int accountId)
        {
            if (transaction.Sender == transaction.Receiver) return transaction.Amount;      //no changes for personal operations

            if (transaction.Sender.Id == accountId && transaction.Amount > 0) return -1 * transaction.Amount;     //Show as loss of personal money;

            if (transaction.Receiver.Id == accountId && transaction.Amount < 0) return -1 * transaction.Amount;   //Should not be. But money to our bill - always positive value

            return transaction.Amount;
        }
    }
}
