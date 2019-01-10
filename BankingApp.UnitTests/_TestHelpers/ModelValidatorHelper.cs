using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankingApp.UnitTests
{
    class ModelValidator
    {
        //https://visualstudiomagazine.com/articles/2015/06/19/tdd-asp-net-mvc-part-4-unit-testing.aspx

        /// <summary>
        /// Validates provided model
        /// </summary>
        /// <param name="model">Model to validate</param>
        /// <returns></returns>
        public static IList<ValidationResult> Validate(object model)
        {
            var results = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, results, true);
            if (model is IValidatableObject) (model as IValidatableObject).Validate(validationContext);
            return results;
        }
    }
}
