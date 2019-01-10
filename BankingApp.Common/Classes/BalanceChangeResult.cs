using BankingApp.DataAccess;

namespace BankingApp.Common
{
    public class BalanceChangeResult
    {
        /// <summary>
        /// Result type
        /// </summary>
        public BalanceChangeResultType Result { get; private set; }

        /// <summary>
        /// Related account
        /// </summary>
        public Account Account { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="result">Result type</param>
        public BalanceChangeResult(BalanceChangeResultType result)
        {
            Result = result;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="result">Result type</param>
        /// <param name="account">Related account</param>
        public BalanceChangeResult(BalanceChangeResultType result, Account account)
        {
            Result = result;
            Account = account;
        }
    }
}
