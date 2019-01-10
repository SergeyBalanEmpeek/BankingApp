using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BankingApp.Common.Messages;
using BankingApp.Services;

namespace BankingApp.WebAPI
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        #region Constructor
        private readonly AccountService _accountService;

        public AuthController(AccountService accountService)
        {
            _accountService = accountService;
        }
        #endregion

        /// <summary>
        /// User is trying to log in
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AuthLoginModel model)
        {
            if (model == null) return BadRequest();

            //Is model valid
            if (ModelState.IsValid == false) return StatusCode(StatusCodes.Status422UnprocessableEntity, ModelState.ListErrors());
            
            //Validate account
            var account = await _accountService.GetAccount(model.Login, model.Password);
            if (account == null) return StatusCode(StatusCodes.Status401Unauthorized, Login.CredentialsInvalid);

            //Success. Return token to a client
            return Ok( new { Token = _accountService.GetAuthToken(account) } );
        }


        /// <summary>
        /// User registration
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns></returns>
        [HttpPost, Route("[action]")]
        public async Task<IActionResult> Create([FromBody]AuthRegistrationModel model)
        {
            if (model == null) return BadRequest();

            //Is model valid
            if (ModelState.IsValid == false) return StatusCode(StatusCodes.Status422UnprocessableEntity, ModelState.ListErrors());

            //Account exists?
            var account = await _accountService.GetAccount(model.Login);
            if (account != null) return StatusCode(StatusCodes.Status409Conflict, Register.LoginExists);

            //Create account
            account = await _accountService.CreateAccount(model.Login, model.Password);
            if (account == null) return StatusCode(StatusCodes.Status500InternalServerError);          //could not be

            //Success. Return token to a client
            return Ok( new { Token = _accountService.GetAuthToken(account) } );
        }
    }
}
