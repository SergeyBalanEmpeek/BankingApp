using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Moq;
using BankingApp.DataAccess;
using BankingApp.Services;
using BankingApp.WebAPI;

namespace BankingApp.UnitTests.Controllers
{
    [TestClass]
    public class AuthControllerTests
    {
        #region LogIn Tests
        [TestMethod]
        public void Post_ModelIsEmpty_ReturnsBadRequest_Test()
        {
            // Arrange
            var mock = new Mock<AccountService>();
            var controller = new AuthController(mock.Object);

            // Act
            var result = controller.Post(null);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
            Assert.AreEqual(StatusCodes.Status400BadRequest, (result.Result as BadRequestResult).StatusCode);
        }

        

        [TestMethod]
        public void Post_LoginPasswordAreInvalid_ReturnsUnauthorized_Test()
        {
            // Arrange
            var mockAccountService = new Mock<AccountService>();
            mockAccountService.Setup(r => r.GetAccount(It.IsAny<string>(), It.IsAny<string>()))
                              .Returns(Task.FromResult<Account>(null));   
            var controller = new AuthController(mockAccountService.Object);
            var model = new AuthLoginModel();

            // Act
            var result = controller.Post(model);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(ObjectResult));
            Assert.AreEqual(StatusCodes.Status401Unauthorized, (result.Result as ObjectResult).StatusCode);
            Assert.AreEqual(Common.Messages.Login.CredentialsInvalid, (result.Result as ObjectResult).Value);
        }

        [TestMethod]
        public void Post_LoginPasswordAreValid_ReturnsOk_Test()
        {
            // Arrange
            var mockAccountService = new Mock<AccountService>();
            mockAccountService.Setup(r => r.GetAccount(It.IsAny<string>(), It.IsAny<string>()))
                              .Returns(Task.FromResult<Account>(new Account()));
            var controller = new AuthController(mockAccountService.Object);
            var model = new AuthLoginModel();

            // Act
            var result = controller.Post(model);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(ObjectResult));
            Assert.AreEqual(StatusCodes.Status200OK, (result.Result as ObjectResult).StatusCode);
        }
        #endregion

        #region Registration Tests
        [TestMethod]
        public void Create_ModelIsEmpty_ReturnsBadRequest_Test()
        {
            // Arrange
            var mock = new Mock<AccountService>();
            var controller = new AuthController(mock.Object);

            // Act
            var result = controller.Create(null);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
            Assert.AreEqual(StatusCodes.Status400BadRequest, (result.Result as BadRequestResult).StatusCode);
        }

        [TestMethod]
        public void Create_LoginIsInUse_ReturnsConflict_Test()
        {
            // Arrange
            var mockAccountService = new Mock<AccountService>();
            mockAccountService.Setup(r => r.GetAccount(It.IsAny<string>()))
                              .Returns(Task.FromResult<Account>(new Account()));
            var controller = new AuthController(mockAccountService.Object);
            var model = new AuthRegistrationModel();

            // Act
            var result = controller.Create(model);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(ObjectResult));
            Assert.AreEqual(StatusCodes.Status409Conflict, (result.Result as ObjectResult).StatusCode);
            Assert.AreEqual(Common.Messages.Register.LoginExists, (result.Result as ObjectResult).Value);
        }


        
        [TestMethod]
        public void Create_WasNotSuccessful_ReturnsError_Test()
        {
            // Arrange
            var mockAccountService = new Mock<AccountService>();
            mockAccountService.Setup(r => r.GetAccount(It.IsAny<string>()))
                              .Returns(Task.FromResult<Account>(null));
            mockAccountService.Setup(r => r.CreateAccount(It.IsAny<string>(), It.IsAny<string>()))
                              .Returns(Task.FromResult<Account>(null));
            var controller = new AuthController(mockAccountService.Object);
            var model = new AuthRegistrationModel();

            // Act
            var result = controller.Create(model);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(StatusCodeResult));
            Assert.AreEqual(StatusCodes.Status500InternalServerError, (result.Result as StatusCodeResult).StatusCode);
        }


        [TestMethod]
        public void Create_WasSuccessful_ReturnsOk_Test()
        {
            // Arrange
            var mockAccountService = new Mock<AccountService>();
            mockAccountService.Setup(r => r.GetAccount(It.IsAny<string>()))
                              .Returns(Task.FromResult<Account>(null));
            mockAccountService.Setup(r => r.CreateAccount(It.IsAny<string>(), It.IsAny<string>()))
                              .Returns(Task.FromResult<Account>(new Account()));
            var controller = new AuthController(mockAccountService.Object);
            var model = new AuthRegistrationModel();

            // Act
            var result = controller.Create(model);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(ObjectResult));
            Assert.AreEqual(StatusCodes.Status200OK, (result.Result as ObjectResult).StatusCode);
        }
        #endregion
    }
}
