using System;
using System.Security.Claims;

namespace BankingApp.WebAPI
{
    public static class ClaimsExtensions
    {
        /// <summary>
        /// Get current User ID 
        /// </summary>
        /// <param name="data">Current claims data</param>
        public static int GetId(this ClaimsPrincipal data)
        {
            //try to find required claim
            Claim claim = data.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null) throw new Exception("Cannot find Name Identifier claim") ;

            //try parse claim value
            if (int.TryParse(claim.Value, out int result) == false) throw new Exception($"Name Identifier is not a number. Actual value is {claim.Value}");
            return result;
        }
    }
}
