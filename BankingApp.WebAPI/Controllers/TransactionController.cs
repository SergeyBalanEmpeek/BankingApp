using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BankingApp.Services;

namespace BankingApp.WebAPI
{
    [Route("api/[controller]")]
    public class TransactionController : Controller
    {
        #region Constructor
        private readonly FinanceService _financeService;

        public TransactionController(FinanceService financeService)
        {
            _financeService = financeService;
        }
        #endregion

        /// <summary>
        /// Get transactions for user. User Id takes from token
        /// </summary>
        /// <returns></returns>
        [HttpGet, Authorize]
        public async Task<IActionResult> Get()
        {
            //Check current user
            var accountId = User.GetId();

            //Get transactions from database and covert to stripped format
            var transactions = await _financeService.GetTransactions(accountId);
            var clientTransations = transactions.Select(c => new TransactionViewModel(c, accountId));

            return Ok(clientTransations);
        }
    }
}
