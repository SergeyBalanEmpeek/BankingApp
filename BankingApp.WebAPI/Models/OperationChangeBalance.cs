using System.ComponentModel.DataAnnotations;

namespace BankingApp.WebAPI
{
    public class OperationChangeBalanceModel
    {
        /// <summary>
        /// How much user wants to deposit/withdraw
        /// </summary>
        [Range(0.01, double.MaxValue, ErrorMessageResourceType = typeof(Common.Messages.Home), ErrorMessageResourceName = nameof(Common.Messages.Home.WrongAmount))] 
        public decimal Amount { get; set; }

        /// <summary>
        /// Current balance, shown to user. Optional. Used to check if user sees latest balance or not.
        /// </summary>
        public decimal? Balance { get; set; }

        /// <summary>
        /// Is operation Withdraw (true) or Deposit (false)
        /// </summary>
        public bool Withdraw { get; set; }
        
        /// <summary>
        /// Corrected amount depending on Withdraw value
        /// </summary>
        public decimal AmountCorrected
        {
            get
            {
                if (Withdraw) return Amount * -1;       //user wants to withdraw money, so value for us is negative
                return Amount;
            }
        }
    }
}
