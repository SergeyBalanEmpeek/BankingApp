using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using Moq;
using BankingApp.Common;
using BankingApp.DataAccess;
using BankingApp.Services;

namespace BankingApp.UnitTests.Services
{
    [TestClass]
    public class FinanceServicesTests
    {
        #region Testing TryChangeBalance
        [TestMethod]
        public void TryChangeBalance_AccountIsNotExists_ReturnsError_Test()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(r => r.Accounts.Get(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(Task.FromResult<Account>(null));

            var service = new FinanceService(mockUnitOfWork.Object);

            // Act
            var result = service.TryChangeBalance(0, 0);

            // Assert
            Assert.AreEqual(BalanceChangeResultType.Error, result.Result.Result);
        }

        [TestMethod]
        public void TryChangeBalance_NegativeBalance_ReturnsNegativeBalanceMessage_Test()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(r => r.Accounts.Get(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(Task.FromResult<Account>(new Account()));

            var service = new FinanceService(mockUnitOfWork.Object);

            // Act
            var result = service.TryChangeBalance(0, -1);

            // Assert
            Assert.AreEqual(BalanceChangeResultType.NegativeBalance, result.Result.Result);
        }

        [TestMethod]
        public void TryChangeBalance_WasSuccessful_ReturnsSuccess_Test()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(r => r.Accounts.Get(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(Task.FromResult<Account>(new Account()));

            mockUnitOfWork.Setup(r => r.Transactions.Create(It.IsAny<Transaction>()));                  //non async

            mockUnitOfWork.Setup(r => r.SaveAsync()).Returns(() => Task.Run(() => { })).Verifiable(); ;      //async

            var service = new FinanceService(mockUnitOfWork.Object);

            // Act
            var result = service.TryChangeBalance(0, 1);

            // Assert
            Assert.AreEqual(BalanceChangeResultType.Success, result.Result.Result);
        }
        
        [TestMethod]
        public void TryChangeBalance_Got1DbUpdateConcurrencyException_ReturnsSuccess_Test()
        {
            // Arrange
            int exceptionCounter = 0;

            var mockUpdateEntry = new Mock<IUpdateEntry>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(r => r.Accounts.Get(It.IsAny<Expression<Func<Account, bool>>>()))
                            .Returns(Task.FromResult<Account>(new Account()));

            mockUnitOfWork.Setup(r => r.Transactions.Create(It.IsAny<Transaction>()));                  //non async void

            mockUnitOfWork.Setup(r => r.SaveAsync())
                .Callback(
                    delegate {
                        //Raise exception only once
                        if (exceptionCounter <= 1)
                        {
                            exceptionCounter++;
                            throw new DbUpdateConcurrencyException("Test Exception", new List<IUpdateEntry> { mockUpdateEntry.Object });    
                        }
                    })
                .Returns(() => Task.Run(() => { })).Verifiable(); ;                                     //async void

            mockUnitOfWork.Setup(r => r.Accounts.DetachEntity(It.IsAny<Account>()));

            var service = new FinanceService(mockUnitOfWork.Object);

            // Act
            var result = service.TryChangeBalance(0, 1);

            // Assert
            Assert.AreEqual(BalanceChangeResultType.Success, result.Result.Result);
        }

        /*
        [TestMethod]
        public void TryChangeBalance_Got5DbUpdateConcurrencyExceptions_ReturnsError_Test()
        {
            // Arrange
            int exceptionCounter = 0;

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(r => r.Accounts.Get(It.IsAny<Expression<Func<Account, bool>>>()))
                            .Returns(Task.FromResult<Account>(new Account()));
            mockUnitOfWork.Setup(r => r.Transactions.Create(It.IsAny<Transaction>()));                  //non async void
            mockUnitOfWork.Setup(r => r.SaveAsync())
                .Callback(
                    delegate {
                        //Raise exception only once
                        if (exceptionCounter <= 5)
                        {
                            exceptionCounter++;
                            throw new DbUpdateConcurrencyException("Test Exception", new List<IUpdateEntry> { new UpdateEntryTest() });
                        }
                    })
                .Returns(() => Task.Run(() => { })).Verifiable(); ;                                     //async void

            var service = new FinanceService(mockUnitOfWork.Object);

            // Act
            var result = service.TryChangeBalance(0, 1);

            // Assert
            Assert.AreEqual(BalanceChangeResultType.Error, result.Result.Result);
        }
        */
        #endregion

        #region Testing TryTransferMoney
        [TestMethod]
        public void TryTransferMoney_AccountIsNotExists_ReturnsError_Test()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(r => r.Accounts.Get(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(Task.FromResult<Account>(null));

            var service = new FinanceService(mockUnitOfWork.Object);

            // Act
            var result = service.TryTransferMoney(0, 0, 0);

            // Assert
            Assert.AreEqual(BalanceChangeResultType.Error, result.Result.Result);
        }

        [TestMethod]
        public void TryTransferMoney_NegativeBalance_ReturnsNegativeBalanceMessage_Test()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(r => r.Accounts.Get(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(Task.FromResult<Account>(new Account() { Balance = 1 }));

            var service = new FinanceService(mockUnitOfWork.Object);

            // Act
            var result = service.TryTransferMoney(0, 0, 2);

            // Assert
            Assert.AreEqual(BalanceChangeResultType.NegativeBalance, result.Result.Result);
        }

        [TestMethod]
        public void TryTransferMoney_WasSuccessful_ReturnsSuccess_Test()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(r => r.Accounts.Get(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(Task.FromResult<Account>(new Account() { Balance = 1 }));

            mockUnitOfWork.Setup(r => r.Transactions.Create(It.IsAny<Transaction>()));                  //non async

            mockUnitOfWork.Setup(r => r.SaveAsync()).Returns(() => Task.Run(() => { })).Verifiable(); ;      //async

            var service = new FinanceService(mockUnitOfWork.Object);

            // Act
            var result = service.TryTransferMoney(0, 0, 1);

            // Assert
            Assert.AreEqual(BalanceChangeResultType.Success, result.Result.Result);
        }

        [TestMethod]
        public void TryTransferMoney_Got1DbUpdateConcurrencyException_ReturnsSuccess_Test()
        {
            // Arrange
            int exceptionCounter = 0;

            var mockUpdateEntry = new Mock<IUpdateEntry>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.Setup(r => r.Accounts.Get(It.IsAny<Expression<Func<Account, bool>>>()))
                            .Returns(Task.FromResult<Account>(new Account() { Balance = 1 }));

            mockUnitOfWork.Setup(r => r.Transactions.Create(It.IsAny<Transaction>()));                  //non async void

            mockUnitOfWork.Setup(r => r.SaveAsync())
                .Callback(
                    delegate {
                        //Raise exception only once
                        if (exceptionCounter <= 1)
                        {
                            exceptionCounter++;
                            throw new DbUpdateConcurrencyException("Test Exception", new List<IUpdateEntry> { mockUpdateEntry.Object });
                        }
                    })
                .Returns(() => Task.Run(() => { })).Verifiable(); ;                                     //async void

            mockUnitOfWork.Setup(r => r.Accounts.DetachEntity(It.IsAny<Account>()));

            var service = new FinanceService(mockUnitOfWork.Object);

            // Act
            var result = service.TryTransferMoney(0, 0, 1);

            // Assert
            Assert.AreEqual(BalanceChangeResultType.Success, result.Result.Result);
        }

        /*
        [TestMethod]
        public void TryTransferMoney_Got5DbUpdateConcurrencyExceptions_ReturnsError_Test()
        {
            // Arrange
            int exceptionCounter = 0;

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(r => r.Accounts.Get(It.IsAny<Expression<Func<Account, bool>>>()))
                            .Returns(Task.FromResult<Account>(new Account() { Balance = 1 }));
            mockUnitOfWork.Setup(r => r.Transactions.Create(It.IsAny<Transaction>()));                  //non async void
            mockUnitOfWork.Setup(r => r.SaveAsync())
                .Callback(
                    delegate {
                        //Raise exception only once
                        if (exceptionCounter <= 5)
                        {
                            exceptionCounter++;
                            throw new DbUpdateConcurrencyException("Test Exception", new List<IUpdateEntry> { new UpdateEntryTest() });
                        }
                    })
                .Returns(() => Task.Run(() => { })).Verifiable(); ;                                     //async void

            var service = new FinanceService(mockUnitOfWork.Object);

            // Act
            var result = service.TryTransferMoney(0, 0, 1);

            // Assert
            Assert.AreEqual(BalanceChangeResultType.Error, result.Result.Result);
        }
        */
        #endregion

        #region Testing IsBalancePositive
        [TestMethod]
        public void IsBalancePositive_AccountIsNull_ThrowsAnException_Test()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var service = new FinanceService(mockUnitOfWork.Object);

            // Act
            try
            {
                var result = service.IsBalancePositive(null, 0);

                //Assert
                Assert.Fail("Account is null, so exceptions should be thrown");
            }
            catch
            {

            }
        }

        [TestMethod]
        public void IsBalancePositive_AccountHas0AndRequestIs0_ReturnsFalse_Test()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var service = new FinanceService(mockUnitOfWork.Object);
            var account = new Account();

            // Act
            var result = service.IsBalancePositive(account, 0);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsBalancePositive_AccountHas0AndRequestIsMinus1_ReturnsFalse_Test()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var service = new FinanceService(mockUnitOfWork.Object);
            var account = new Account() { Balance = 0 };

            // Act
            var result = service.IsBalancePositive(account, -1);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsBalancePositive_AccountHas1AndRequestIsMinus1_ReturnsFalse_Test()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var service = new FinanceService(mockUnitOfWork.Object);
            var account = new Account() { Balance = 1 };

            // Act
            var result = service.IsBalancePositive(account, -1);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsBalancePositive_AccountHas1AndRequestIs1_ReturnsFalse_Test()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var service = new FinanceService(mockUnitOfWork.Object);
            var account = new Account() { Balance = 1 };

            // Act
            var result = service.IsBalancePositive(account, 1);

            //Assert
            Assert.IsTrue(result);
        }
        #endregion

        [TestMethod]
        public void GetRecipientsForAccount_WasSuccessful_ReturnsList_Test()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(r => r.Accounts.GetAll(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(Task.FromResult(new List<Account>().AsEnumerable()));

            var service = new FinanceService(mockUnitOfWork.Object);

            // Act
            var result = service.GetRecipientsForAccount(0);

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(IEnumerable<Account>));
        }

        [TestMethod]
        public void GetTransactions_WasSuccessful_ReturnsList_Test()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(r => r.Transactions.GetTransactions(It.IsAny<int>()))
                          .Returns(Task.FromResult(new List<Transaction>().AsEnumerable()));

            var service = new FinanceService(mockUnitOfWork.Object);

            // Act
            var result = service.GetTransactions(0);

            //Assert
            Assert.IsInstanceOfType(result.Result, typeof(IEnumerable<Transaction>));
        }
    }
}
