using CoreBanking.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreBanking.API.DAL
{
    public class CoreBankingDbContext :  DbContext
    {
        public CoreBankingDbContext(DbContextOptions<CoreBankingDbContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
