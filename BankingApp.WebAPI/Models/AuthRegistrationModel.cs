using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BankingApp.WebAPI
{
    public class AuthRegistrationModel
    {
        /// <summary>
        /// Login
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Common.Messages.Register), ErrorMessageResourceName = nameof(Common.Messages.Register.LoginRequired))]
        [RegularExpression(pattern: "^[a-zA-Z0-9]*$", ErrorMessageResourceType = typeof(Common.Messages.Register), ErrorMessageResourceName = nameof(Common.Messages.Register.LoginInvalid))]
        public string Login { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Common.Messages.Register), ErrorMessageResourceName = nameof(Common.Messages.Register.PasswordRequired))]
        [MinLength(length: 6, ErrorMessageResourceType = typeof(Common.Messages.Register), ErrorMessageResourceName = nameof(Common.Messages.Register.PasswordShort))]
        [JsonProperty(PropertyName = "new-password")]
        public string Password { get; set; }
    }
}
