using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.DataAccess
{
    public class AccountRepository : GenericRepository<Account>
    {
        private readonly DbContext _dbContext;
        public AccountRepository(DbContext dbContext): base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// This constructor is for Unit Testing purposes only
        /// </summary>
        public AccountRepository(): base(null)
        {

        }
    }
}
