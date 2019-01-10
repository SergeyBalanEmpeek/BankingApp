using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BankingApp.WebAPI
{
    public static class ModelStateExcentions
    {
        /// <summary>
        /// Enumerate all existing model errors
        /// </summary>
        /// <param name="ModelState">model state instance</param>
        /// <returns></returns>
        public static string ListErrors(this ModelStateDictionary modelState)
        {
            //select all existing errors at array of strings
            var errors = modelState.Values.SelectMany(v => v.Errors).Select(c => c.ErrorMessage);

            //join them into single line
            return string.Join(" ", errors);
        }
    }
}
