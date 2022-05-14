using CoreBanking.API.DAL;
using CoreBanking.API.Models;
using CoreBanking.API.Services.Interfaces;
using System.Collections.Generic;

namespace CoreBanking.API.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly CoreBankingDbContext _dbContext;

        // Dependency Injection in Class Constructor
        // تزریق وابستگی درون سازنده کلاس 
        public AccountService(CoreBankingDbContext dbContext)
        {
            _dbContext = dbContext;
        }
       
        public Account Authenticate(string AccountNumber, string pin)
        {
            throw new System.NotImplementedException();
        }

        public Account Create(Account account, string pin, string ConfirmPin)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int Id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            throw new System.NotImplementedException();
        }

        public Account GetByAccountNumber(string Accountnumber)
        {
            throw new System.NotImplementedException();
        }

        public Account GetById(int Id)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Account account, string pin = null)
        {
            throw new System.NotImplementedException();
        }
    }
}
