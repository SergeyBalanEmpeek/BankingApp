using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using BankingApp.DataAccess;

namespace BankingApp.Services
{
    public class AccountService
    {
        /// <summary>
        /// Database instance
        /// </summary>
        private readonly IUnitOfWork _db;

        /// <summary>
        /// Configuration instance
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db">Database instance</param>
        /// <param name="configuration">configuration instance</param>
        public AccountService(IUnitOfWork db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        /// <summary>
        /// This constructor is for Unit Testing purposes only
        /// </summary>
        public AccountService()
        {
        }

        /// <summary>
        /// Get Account by Login
        /// </summary>
        /// <param name="login">Login</param>
        /// <returns>Validated account</returns>
        public async virtual Task<Account> GetAccount(string login)
        {
            //Basic checking
            if (string.IsNullOrEmpty(login) == true) return null;

            return await _db.Accounts.Get(c => c.Login == login.Trim());
        }

        /// <summary>
        /// Get Account by Account ID
        /// </summary>
        /// <param name="accountId">Account ID</param>
        /// <returns>Validated account</returns>
        public async virtual Task<Account> GetAccount(int accountId)
        {
            return await _db.Accounts.Get(c => c.Id == accountId);
        }

        /// <summary>
        /// Get Account by Login and Password
        /// </summary>
        /// <param name="login">Login</param>
        /// <param name="password">Password</param>
        /// <returns>Validated account</returns>
        public async virtual Task<Account> GetAccount(string login, string password)
        {
            //Basic checking
            if (string.IsNullOrEmpty(login) == true) return null;
            if (string.IsNullOrEmpty(password) == true) return null;

            //Find user
            var account = await _db.Accounts.Get(c => c.Login == login.Trim());
            if (account == null) return null;
            
            //Is password correct
            if (account.PasswordHash != GetHash(password, account.PasswordSalt)) return null;

            return account;
        }

        /// <summary>
        /// Register new account
        /// </summary>
        /// <param name="login">Login</param>
        /// <param name="password">Password</param>
        /// <returns>Validated account</returns>
        public async virtual Task<Account> CreateAccount(string login, string password)
        {
            //Basic checking
            if (string.IsNullOrEmpty(login) == true) return null;
            if (string.IsNullOrEmpty(password) == true) return null;

            //Prepare password
            string salt = GetSalt();
            string hash = GetHash(password, salt);

            //Create new user
            var account = new Account
            {
                Login = login.Trim(),
                PasswordHash = hash,
                PasswordSalt = salt,
                Balance = 0
            };

            //Store to Database
            _db.Accounts.Create(account);
            await _db.SaveAsync();
            
            return account;
        }

        /// <summary>
        /// Get JSON Web Token for specified account
        /// </summary>
        /// <param name="account">Account to provide JWT</param>
        /// <returns></returns>
        public virtual string GetAuthToken(Account account)
        {
            if (account == null) return null;

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            //Data to store in JWT
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Name, account.Login),
                new Claim(ClaimTypes.Role, "User")
            };

            var tokeOptions = new JwtSecurityToken(
                issuer: _configuration["Tokens:Issuer"],
                audience: _configuration["Tokens:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: signinCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        }

        #region Helpers
        /// <summary>
        /// Generate salt
        /// </summary>
        /// <returns></returns>
        private string GetSalt()
        {
            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }

        /// <summary>
        /// Generate hash
        /// </summary>
        /// <param name="value">value to hash</param>
        /// <param name="salt">salt for hash</param>
        /// <returns></returns>
        private string GetHash(string value, string salt)
        {
            //https://docs.microsoft.com/ru-ru/aspnet/core/security/data-protection/consumer-apis/password-hashing

            byte[] hashed = KeyDerivation.Pbkdf2(
                    password: value,
                    salt: Convert.FromBase64String(salt),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8);

            return Convert.ToBase64String(hashed);
        }
        #endregion
    }
}
