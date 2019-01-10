using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using BankingApp.Common;
using BankingApp.Common.Messages;
using BankingApp.Services;

namespace BankingApp.WebAPI
{
    [Route("api/[controller]")]
    public class OperationController : Controller
    {
        #region Constructor
        private readonly AccountService _accountService;
        private readonly FinanceService _financeService;

        public OperationController(AccountService accountService, FinanceService financeService)
        {
            _accountService = accountService;
            _financeService = financeService;
        }
        #endregion

        /// <summary>
        /// User wants to deposit/withdraw money
        /// </summary>
        /// <returns></returns>
        [HttpPost, Authorize, Route("{action}")]
        public async Task<IActionResult> Balance([FromBody]OperationChangeBalanceModel model)
        {
            if (model == null) return BadRequest();

            //Is model valid
            if (ModelState.IsValid == false) return StatusCode(StatusCodes.Status422UnprocessableEntity, ModelState.ListErrors());

            //Try to change balance
            var result = await _financeService.TryChangeBalance(User.GetId(), model.AmountCorrected);
            switch(result.Result)
            {
                case BalanceChangeResultType.Success:
                    return Ok(new { result.Account.Login, result.Account.Balance });

                case BalanceChangeResultType.NegativeBalance:
                    return StatusCode(StatusCodes.Status409Conflict, new { result.Account.Login, result.Account.Balance });   
                    
                default:
                case BalanceChangeResultType.Error:
                    return StatusCode(StatusCodes.Status500InternalServerError, Home.GenericError);
            }
        }

        /// <summary>
        /// User wants to transfer money to another account
        /// </summary>
        /// <returns></returns>
        [HttpPost, Authorize, Route("{action}")]
        public async Task<IActionResult> Transfer([FromBody]OperationTransferMoneyModel model)
        {
            if (model == null) return BadRequest();

            //Is model valid
            if (ModelState.IsValid == false) return StatusCode(StatusCodes.Status422UnprocessableEntity, ModelState.ListErrors());

            //Try to transfer money
            var result = await _financeService.TryTransferMoney(User.GetId(), model.Receiver, model.Amount);
            switch (result.Result)
            {
                case BalanceChangeResultType.Success:
                    return Ok(new { result.Account.Login, result.Account.Balance });

                case BalanceChangeResultType.NegativeBalance:
                    //Balance just changed to a new value in other thread. Warn user. Send an updated balance value
                    return StatusCode(StatusCodes.Status409Conflict, new { result.Account.Login, result.Account.Balance });

                default:
                case BalanceChangeResultType.Error:
                    return StatusCode(StatusCodes.Status500InternalServerError, Home.GenericError);
            }
        }
    }
}
