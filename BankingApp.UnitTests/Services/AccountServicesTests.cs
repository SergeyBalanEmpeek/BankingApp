using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using Moq;
using BankingApp.DataAccess;
using BankingApp.Services;

namespace BankingApp.UnitTests.Services
{
    [TestClass]
    public class AccountServicesTests
    {
        #region Testing GetAccount(int id)
        [TestMethod]
        public void GetAccountById_IdIsNotExists_ReturnsNull_Test()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(r => r.Accounts.Get(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(Task.FromResult<Account>(null));    //return null

            var mockConfiguration = new Mock<IConfiguration>();

            var service = new AccountService(mockUnitOfWork.Object, mockConfiguration.Object);

            // Act
            var result = service.GetAccount(0);

            // Assert
            Assert.AreEqual(null, result.Result);
        }

        [TestMethod]
        public void GetAccountById_IdIsExists_ReturnsAccount_Test()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(r => r.Accounts.Get(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(Task.FromResult<Account>(new Account())); 

            var mockConfiguration = new Mock<IConfiguration>();

            var service = new AccountService(mockUnitOfWork.Object, mockConfiguration.Object);

            // Act
            var result = service.GetAccount(0);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(Account));
        }
        #endregion

        #region Testing GetAccount(string login)
        [TestMethod]
        public void GetAccountByLogin_LoginIsNull_ReturnsNull_Test()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(r => r.Accounts.Get(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(Task.FromResult<Account>(null));   

            var mockConfiguration = new Mock<IConfiguration>();

            var service = new AccountService(mockUnitOfWork.Object, mockConfiguration.Object);

            // Act
            var result = service.GetAccount(null);

            // Assert
            Assert.AreEqual(null, result.Result);
        }

        [TestMethod]
        public void GetAccountByLogin_LoginIsEmpty_ReturnsNull_Test()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(r => r.Accounts.Get(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(Task.FromResult<Account>(null));

            var mockConfiguration = new Mock<IConfiguration>();

            var service = new AccountService(mockUnitOfWork.Object, mockConfiguration.Object);

            // Act
            var result = service.GetAccount("");

            // Assert
            Assert.AreEqual(null, result.Result);
        }


        [TestMethod]
        public void GetAccountByLogin_LoginIsNotExists_ReturnsNull_Test()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(r => r.Accounts.Get(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(Task.FromResult<Account>(null));

            var mockConfiguration = new Mock<IConfiguration>();

            var service = new AccountService(mockUnitOfWork.Object, mockConfiguration.Object);

            // Act
            var result = service.GetAccount("1");

            // Assert
            Assert.AreEqual(null, result.Result);
        }

        [TestMethod]
        public void GetAccountByLogin_LoginIsExists_ReturnsAccount_Test()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(r => r.Accounts.Get(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(Task.FromResult<Account>(new Account()));

            var mockConfiguration = new Mock<IConfiguration>();

            var service = new AccountService(mockUnitOfWork.Object, mockConfiguration.Object);

            // Act
            var result = service.GetAccount("1");

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(Account));
        }
        #endregion

        #region Testing GetAccount(string login, string password)
        [TestMethod]
        public void GetAccountByLoginPassword_LoginIsNull_ReturnsNull_Test()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(r => r.Accounts.Get(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(Task.FromResult<Account>(null));

            var mockConfiguration = new Mock<IConfiguration>();

            var service = new AccountService(mockUnitOfWork.Object, mockConfiguration.Object);

            // Act
            var result = service.GetAccount(null, "1");

            // Assert
            Assert.AreEqual(null, result.Result);
        }

        [TestMethod]
        public void GetAccountByLoginPassword_PasswordIsNull_ReturnsNull_Test()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(r => r.Accounts.Get(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(Task.FromResult<Account>(null));

            var mockConfiguration = new Mock<IConfiguration>();

            var service = new AccountService(mockUnitOfWork.Object, mockConfiguration.Object);

            // Act
            var result = service.GetAccount("1", null);

            // Assert
            Assert.AreEqual(null, result.Result);
        }

        [TestMethod]
        public void GetAccountByLoginPassword_LoginPasswordAreInvalid_ReturnsNull_Test()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(r => r.Accounts.Get(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(Task.FromResult<Account>(null));

            var mockConfiguration = new Mock<IConfiguration>();

            var service = new AccountService(mockUnitOfWork.Object, mockConfiguration.Object);

            // Act
            var result = service.GetAccount("1", "1");

            // Assert
            Assert.AreEqual(null, result.Result);
        }

        [TestMethod]
        public void GetAccountByLoginPassword_LoginPasswordAreValid_ReturnsNull_Test()
        {
            // Arrange
            Account account = new Account() { Login = "UserA", PasswordHash = "lKw6b3NHk8uyGrBkzhj5Ge6XhZhTtB2sYAYAJ7ZI6UQ=", PasswordSalt = "lA6lNwKShjkYMfog/2QkqQ==" };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(r => r.Accounts.Get(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(Task.FromResult<Account>(account));

            var mockConfiguration = new Mock<IConfiguration>();

            var service = new AccountService(mockUnitOfWork.Object, mockConfiguration.Object);

            // Act
            var result = service.GetAccount("UserA", "111111");

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(Account));
            Assert.AreEqual(account.Login, result.Result.Login);
        }
        #endregion
    }
}
