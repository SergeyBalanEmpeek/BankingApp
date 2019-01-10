using Microsoft.EntityFrameworkCore;
using BankingApp.DataAccess;
using Microsoft.Extensions.Configuration;


namespace BankingApp.UnitIntegrationTests
{
    class DatabaseConnector
    {
        public UnitOfWork GetInstance()
        {
            //Prepare configuration builder instance
            var configuration = ConfigBuilder.GetInstance();

            //Prepare options to connect to db
            DbContextOptions<BankingContext> options = new DbContextOptionsBuilder<BankingContext>()
                                                        .UseSqlServer(configuration.GetConnectionString("DefaultConnection")).Options;

            //Init db context
            BankingContext content = new BankingContext(options);

            //Return unit of work instance
            return new UnitOfWork(content);
        }
    }
}
