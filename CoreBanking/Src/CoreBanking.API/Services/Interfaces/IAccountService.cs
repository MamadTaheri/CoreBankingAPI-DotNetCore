using CoreBanking.API.Models;
using System.Collections.Generic;

namespace CoreBanking.API.Services.Interfaces
{
    public interface IAccountService
    {
        Account Authenticate(string AccountNumber, string pin);
        IEnumerable<Account> GetAllAccounts();
        Account Create(Account account, string pin, string ConfirmPin);
        void Update(Account account, string pin = null);
        void Delete(int Id);
        Account GetById(int Id);
        Account GetByAccountNumber(string Accountnumber);
    }
}
