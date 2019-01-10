using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BankingApp.Services;

namespace BankingApp.WebAPI
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        #region Constructor
        private readonly AccountService _accountService;
        private readonly FinanceService _financeService;

        public AccountController(AccountService accountService, FinanceService financeService)
        {
            _accountService = accountService;
            _financeService = financeService;
        }
        #endregion
        
        /// <summary>
        /// Get user details. User Id takes from token
        /// </summary>
        /// <returns></returns>
        [HttpGet, Authorize]

        public async Task<IActionResult> Get()
        {
            //Get current user
            var account = await _accountService.GetAccount(User.GetId());
            if (account == null) return Unauthorized();

            //Get recipients for this account
            var recipients = await _financeService.GetRecipientsForAccount(account.Id);

            //Prepare results
            var resultUser = new { account.Login, account.Balance };
            var resultRecipients = from item in recipients select new { id = item.Id, name = item.Login };

            return Ok( new { account = resultUser, recipients = resultRecipients });
        }
    }
}
