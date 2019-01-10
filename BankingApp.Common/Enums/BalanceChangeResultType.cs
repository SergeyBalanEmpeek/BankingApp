namespace BankingApp.Common
{
    /// <summary>
    /// Result type of money transfer
    /// </summary>
    public enum BalanceChangeResultType
    {
        /// <summary>
        /// Successful result
        /// </summary>
        Success = 1,

        /// <summary>
        /// Balance is lower than zero
        /// </summary>
        NegativeBalance = 2,

        /// <summary>
        /// Error situation
        /// </summary>
        Error = 3
    }
}
