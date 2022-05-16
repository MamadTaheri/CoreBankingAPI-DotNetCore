using CoreBanking.API.DAL;
using CoreBanking.API.Models;
using CoreBanking.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            var account = _dbContext.Accounts.Where(q => q.AccountNumberGenerated == AccountNumber).SingleOrDefault();

            // Validation
            if (account == null)
            {
                return null;
            }
            if (!VerifyPinHash(pin, account.PinHash, account.PinSalt))
            {
                return null;
            }

            // Do Authenticate
            return account;
        }

        private static bool VerifyPinHash(string Pin, byte[] pinHash, byte[] pinSalt)
        {
            if (string.IsNullOrWhiteSpace(Pin)) throw new ArgumentNullException("Pin");
            using (var hmac = new System.Security.Cryptography.HMACSHA512(pinSalt))
            {
                var computedPinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(Pin));
                for (int i = 0; i < computedPinHash.Length; i++)
                {
                    if (computedPinHash[i] != pinHash[i]) return false;
                }
            }
            return true;
        }

        public Account Create(Account account, string Pin, string ConfirmPin)
        {
            // Validation
            if (_dbContext.Accounts.Any(q => q.Email == account.Email))
            {
                throw new ApplicationException("An account already exists with this email");
            }
            if (!Pin.Equals(ConfirmPin))
            {
                throw new ArgumentException("Pins do not match");
            }

            // Create
            byte[] pinHash, pinSalt;
            CreatePinHash(Pin, out pinHash, out pinSalt);
            account.PinHash = pinHash;
            account.PinSalt = pinSalt;
            _dbContext.Accounts.Add(account);
            _dbContext.SaveChanges();
            return account;
        }

        private static void CreatePinHash(string pin, out byte[] pinHash, out byte[] pinSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                pinSalt = hmac.Key;
                pinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
            }
        }

        public void Delete(int Id)
        {
            var account = _dbContext.Accounts.Find(Id);
            if (account != null)
            {
                _dbContext.Accounts.Remove(account);
                _dbContext.SaveChanges();
            }
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return _dbContext.Accounts.ToList();
        }

        public Account GetByAccountNumber(string Accountnumber)
        {
            var account = _dbContext.Accounts.Where(q => q.AccountNumberGenerated == Accountnumber).FirstOrDefault();
            if (account == null)
            {
                return null;
            }
            return account;
        }

        public Account GetById(int Id)
        {
            var account = _dbContext.Accounts.Where(q => q.Id == Id).FirstOrDefault();
            if (account == null)
            {
                return null;
            }
            return account;
        }

        public void Update(Account account, string pin = null)
        {
            var accountToBeUpdated = _dbContext.Accounts.Find(account.Id);
            if (accountToBeUpdated == null)
            {
                throw new ApplicationException("Account does not exist");
            }
            if (!string.IsNullOrWhiteSpace(account.Email))
            {
                if (_dbContext.Accounts.Any(q => q.Email == account.Email))
                    throw new ApplicationException("this email " + account.Email + " already exists");
                accountToBeUpdated.Email = account.Email;
            }
            if (!string.IsNullOrWhiteSpace(account.PhoneNumber))
            {
                if (_dbContext.Accounts.Any(q => q.PhoneNumber == account.PhoneNumber))
                    throw new ApplicationException("this PhoneNumber " + account.PhoneNumber + " already exists");
                accountToBeUpdated.PhoneNumber = account.PhoneNumber;
            }
            if(!string.IsNullOrWhiteSpace(pin))
            {
                byte[] pinHash, pinSalt;
                CreatePinHash(pin, out pinHash, out pinSalt);
                accountToBeUpdated.PinHash = pinHash;
                accountToBeUpdated.PinSalt = pinSalt;
            }
            accountToBeUpdated.DateLastUpdated = DateTime.Now;
            _dbContext.Accounts.Update(accountToBeUpdated);
            _dbContext.SaveChanges();
        }
    }
}
