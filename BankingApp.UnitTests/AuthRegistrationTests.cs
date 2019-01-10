using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Moq;
using BankingApp.DataAccess;
using BankingApp.Services;
using BankingApp.WebAPI;

namespace BankingApp.UnitTests
{
    [TestClass]
    public class AuthRegistrationTests
    {       
        [TestMethod]
        public void ModelNull()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            var accountService = new AccountService(mock.Object, AppSettings.Get());
            var controller = new AuthController(accountService);

            // Act
            var result = controller.Create(null);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
            Assert.AreEqual(StatusCodes.Status400BadRequest, (result.Result as BadRequestResult).StatusCode);
        }
        [TestMethod]
        public void UserExists()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(r => r.Accounts.Get(It.IsAny<Expression<Func<Account, bool>>>()))
                .Returns(Task.FromResult(new Account()));    //return 1 existing account
            var accountService = new AccountService(mock.Object, AppSettings.Get());
            var controller = new AuthController(accountService);
            var model = new AuthRegistrationModel() { Login = "UserA", Password = "111111" };

            // Act
            var result = controller.Create(model);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(ObjectResult));
            Assert.AreEqual(StatusCodes.Status409Conflict, (result.Result as ObjectResult).StatusCode);
            Assert.AreEqual(Common.Messages.Register.LoginExists, (result.Result as ObjectResult).Value);
        }

        [TestMethod]
        public void Success()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(r => r.Accounts.Get(It.IsAny<Expression<Func<Account, bool>>>()))
                .Returns(Task.FromResult<Account>(null));    //return no accounts
            mock.Setup(r => r.Accounts.Create(It.IsAny<Account>()));
            var accountService = new AccountService(mock.Object, AppSettings.Get());
            var controller = new AuthController(accountService);
            var model = new AuthRegistrationModel() { Login = "UserA", Password = "111111" };

            // Act
            var result = controller.Create(model);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(ObjectResult));
            Assert.AreEqual(StatusCodes.Status200OK, (result.Result as ObjectResult).StatusCode);
        }
    }
}
