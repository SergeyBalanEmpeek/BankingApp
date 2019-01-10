using Microsoft.VisualStudio.TestTools.UnitTesting;
using BankingApp.WebAPI;

namespace BankingApp.UnitTests.Models
{
    [TestClass]
    public class AuthLoginModelTests
    {
        private const string validLogin = "1";
        private const string validPassword = "111111";

        [TestMethod]
        public void IsValid_LoginIsNull_ReturnsLoginIsRequired_Test()
        {
            // Arrange
            var model = new AuthLoginModel() { Login = null, Password = validPassword };

            // Act
            var result = ModelValidator.Validate(model);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(Common.Messages.Login.LoginRequired, result[0].ErrorMessage);
        }

        [TestMethod]
        public void IsValid_LoginIsEmpty_ReturnsLoginIsRequired_Test()
        {
            // Arrange
            var model = new AuthLoginModel() { Login = "", Password = validPassword };

            // Act
            var result = ModelValidator.Validate(model);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(Common.Messages.Login.LoginRequired, result[0].ErrorMessage);
        }

        [TestMethod]
        public void IsValid_PasswordIsNull_ReturnsPasswordIsRequired_Test()
        {
            // Arrange
            var model = new AuthLoginModel() { Login = validLogin, Password = null };

            // Act
            var result = ModelValidator.Validate(model);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(Common.Messages.Login.PasswordRequired, result[0].ErrorMessage);
        }

        [TestMethod]
        public void IsValid_PasswordIsEmpty_ReturnsPasswordIsRequired_Test()
        {
            // Arrange
            var model = new AuthLoginModel() { Login = validLogin, Password = "" };

            // Act
            var result = ModelValidator.Validate(model);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(Common.Messages.Login.PasswordRequired, result[0].ErrorMessage);
        }

        [TestMethod]
        public void IsValid_CorrectModel_ReturnsNoErrors_Test()
        {
            // Arrange
            var model = new AuthLoginModel() { Login = validLogin, Password = validPassword };

            // Act
            var result = ModelValidator.Validate(model);

            // Assert
            Assert.AreEqual(0, result.Count);
        }
    }
}
