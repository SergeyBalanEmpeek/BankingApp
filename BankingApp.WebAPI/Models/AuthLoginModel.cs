using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BankingApp.WebAPI
{
    public class AuthLoginModel
    {
        /// <summary>
        /// Login
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Common.Messages.Login), ErrorMessageResourceName = nameof(Common.Messages.Login.LoginRequired))]
        public string Login { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Common.Messages.Login), ErrorMessageResourceName = nameof(Common.Messages.Login.PasswordRequired))]
        [JsonProperty(PropertyName = "current-password")]
        public string Password { get; set; }
        /// <summary>
    }
}
