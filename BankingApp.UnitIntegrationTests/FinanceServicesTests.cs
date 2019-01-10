using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BankingApp.DataAccess;
using BankingApp.Services;

namespace BankingApp.UnitIntegrationTests
{
    [TestClass]
    public class FinanceServicesTests
    {
        //private TransactionScope trans = null;

        [TestInitialize]
        public void TestInitialize()
        {
            //trans = new TransactionScope(TransactionScopeOption.Required);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            //trans.Dispose();
        }

        [TestMethod]
        public void TryChangeBalance_AsyncRequests_Passes_Test()
        {
            // Arrange

            //Init two different instances of unit of work
            UnitOfWork db1 = new DatabaseConnector().GetInstance();
            UnitOfWork db2 = new DatabaseConnector().GetInstance();

            //Init two different instances services - with different unit of works
            FinanceService fs1 = new FinanceService(db1);
            FinanceService fs2 = new FinanceService(db2);

            //Reset balance for test account
            Account user = db1.Accounts.Get(c => c.Id == 1).Result;
            user.Balance = 0;                //reset balance
            db1.Save();                      //save it

            //Prepare threads

            var task1 = Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    var result = fs1.TryChangeBalance(user.Id, 1).Result;
                }
            });
            
            var task2 = Task.Run(() =>
            {
                var result = fs2.TryChangeBalance(user.Id, 2).Result;
            });
            
            bool task1Completed = task1.Wait(TimeSpan.FromMilliseconds(5000));
            bool task2Completed = task2.Wait(TimeSpan.FromMilliseconds(5000));

            //Get updated value from database using a new db instance
            UnitOfWork db3 = new DatabaseConnector().GetInstance();
            var actualUser = db3.Accounts.Get(c => c.Id == user.Id).Result;

            Assert.AreEqual(10 * 1 + 2, actualUser.Balance, "Actual value check fail");    //both 2 threads should complete
        }

        
        [TestMethod]
        public void TryTransferMoney_AsyncRequests_Passes_Test()
        {
            // Arrange

            //Init two different instances of unit of work
            UnitOfWork db1 = new DatabaseConnector().GetInstance();
            UnitOfWork db2 = new DatabaseConnector().GetInstance();

            //Init two different instances services - with different unit of works
            FinanceService fs1 = new FinanceService(db1);
            FinanceService fs2 = new FinanceService(db2);

            //Reset balance for test accounts
            Account sender = db1.Accounts.Get(c => c.Id == 1).Result;
            sender.Balance = 100;                //reset balance

            Account receiver = db1.Accounts.Get(c => c.Id == 2).Result;
            receiver.Balance = 0;                //reset balance

            db1.Save();                          //save it

            //Prepare threads

            var task1 = Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    var result = fs1.TryTransferMoney(sender.Id, receiver.Id, 1).Result;
                }
            });

            var task2 = Task.Run(() =>
            {
                var result = fs2.TryTransferMoney(sender.Id, receiver.Id, 2).Result;
            });

            bool task1Completed = task1.Wait(TimeSpan.FromMilliseconds(5000));
            bool task2Completed = task2.Wait(TimeSpan.FromMilliseconds(5000));

            //Get updated value from database using a new db instance
            UnitOfWork db3 = new DatabaseConnector().GetInstance();
            var actualSender = db3.Accounts.Get(c => c.Id == sender.Id).Result;
            var actualReceiver = db3.Accounts.Get(c => c.Id == receiver.Id).Result;

            int correct = 10 * 1 + 2;
            Assert.AreEqual(correct, actualReceiver.Balance, "Actual value fail (receiver)");    //both 2 threads should complete
            Assert.AreEqual(100 - correct, actualSender.Balance, "Actual value fail (sender)");
        }
    }
}
