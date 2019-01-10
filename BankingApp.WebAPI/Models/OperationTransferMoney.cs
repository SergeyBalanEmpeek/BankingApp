using System.ComponentModel.DataAnnotations;

namespace BankingApp.WebAPI
{
    public class OperationTransferMoneyModel
    {
        /// <summary>
        /// How much user wants to transfer
        /// </summary>
        [Range(0.01, double.MaxValue, ErrorMessageResourceType = typeof(Common.Messages.Home), ErrorMessageResourceName = nameof(Common.Messages.Home.WrongAmount))]
        public decimal Amount { get; set; }

        /// <summary>
        /// Money receiver
        /// </summary>
        public int Receiver { get; set; }

        /// <summary>
        /// Current balance, shown to user. Optional. Used to check if user sees latest balance or not.
        /// </summary>
        public decimal? Balance { get; set; }
    }
}
