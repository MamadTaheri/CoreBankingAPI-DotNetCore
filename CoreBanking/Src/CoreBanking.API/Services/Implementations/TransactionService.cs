using CoreBanking.API.DAL;
using CoreBanking.API.Models;
using CoreBanking.API.Services.Interfaces;
using CoreBanking.API.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace CoreBanking.API.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        private readonly CoreBankingDbContext _dbContext;
        ILogger<TransactionService> _logger;
        private AppSettings _settings;
        private static string _ourBankSettlementAccount;
        private readonly IAccountService _accountService;

        public TransactionService(
            CoreBankingDbContext context, 
            ILogger<TransactionService> logger, 
            IOptions<AppSettings> settings, 
            IAccountService accountService)
        {
            _dbContext = context;
            _logger = logger;
            _settings = settings.Value;
            _ourBankSettlementAccount = _settings.OurBankSettlementAccount;
            _accountService = accountService;
        }
        public ResponseDTO CreateNewTransaction(Transaction transaction)
        {
            ResponseDTO response = new ResponseDTO();
            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();
            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction Created Successfully";
            response.Data = null;
            return response;
        }

        public ResponseDTO FindTransactionByDate(DateTime date)
        {
            ResponseDTO response = new ResponseDTO();
            var transactionList = _dbContext.Transactions.Where(q => q.TransactionDate == date).ToList();
            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction Created Successfully";
            response.Data = transactionList;
            return response;
        }

        public ResponseDTO MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin)
        {
            ResponseDTO response = new ResponseDTO();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            var authUser = _accountService.Authenticate(AccountNumber, TransactionPin);
            if (authUser == null)
                throw new ApplicationException("Invalid credentials");

            try
            {
                sourceAccount = _accountService.GetByAccountNumber(_ourBankSettlementAccount);
                destinationAccount = _accountService.GetByAccountNumber(AccountNumber);

                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                if((_dbContext.Entry(sourceAccount).State == EntityState.Modified) && 
                    (_dbContext.Entry(destinationAccount).State == EntityState.Modified ))
                {
                    // Successfull
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction Successfull!";
                    response.Data = null;
                } else
                {
                    // Fail
                    transaction.TransactionStatus = TranStatus.Failed;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction failed`!";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"AN ERROR OCCURRED... => { ex.Message}");
            }

            transaction.TransactionType = TranType.Deposit;
            transaction.TransactionSourceAccount = _ourBankSettlementAccount;
            transaction.TransactionDestinationAccount = AccountNumber;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"NEW TRANSACTION " +
                $"FROM SOURCE => {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} " +
                $" TO DESTINATION ACCOUNT => {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} " +
                $" ON DATE {transaction.TransactionDate} FOR AMOUNT => {JsonConvert.SerializeObject(transaction.TransactionAmount)} " +
                $" TRANSACTION TYPE => {JsonConvert.SerializeObject(transaction.TransactionType)}" +
                $" TRANSACTION STATUS => {JsonConvert.SerializeObject(transaction.TransactionStatus)}";

            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();
            
            return response;

        }

        public ResponseDTO MakeFundsTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
        {
            ResponseDTO response = new ResponseDTO();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            var authUser = _accountService.Authenticate(FromAccount, TransactionPin);
            if (authUser == null)
                throw new ApplicationException("Invalid credentials");

            try
            {
                sourceAccount = _accountService.GetByAccountNumber(FromAccount);
                destinationAccount = _accountService.GetByAccountNumber(ToAccount);

                sourceAccount.CurrentAccountBalance += Amount;
                destinationAccount.CurrentAccountBalance -= Amount;

                if ((_dbContext.Entry(sourceAccount).State == EntityState.Modified) &&
                    (_dbContext.Entry(destinationAccount).State == EntityState.Modified))
                {
                    // Successfull
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction Successfull!";
                    response.Data = null;
                }
                else
                {
                    // Fail
                    transaction.TransactionStatus = TranStatus.Failed;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction failed`!";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"AN ERROR OCCURRED... => { ex.Message}");
            }

            transaction.TransactionType = TranType.Transfer;
            transaction.TransactionSourceAccount = FromAccount;
            transaction.TransactionDestinationAccount = ToAccount;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"NEW TRANSACTION " +
                $"FROM SOURCE => {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} " +
                $" TO DESTINATION ACCOUNT => {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} " +
                $" ON DATE {transaction.TransactionDate} FOR AMOUNT => {JsonConvert.SerializeObject(transaction.TransactionAmount)} " +
                $" TRANSACTION TYPE => {JsonConvert.SerializeObject(transaction.TransactionType)}" +
                $" TRANSACTION STATUS => {JsonConvert.SerializeObject(transaction.TransactionStatus)}";

            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();

            return response;
        }

        public ResponseDTO MakeWithDrawal(string AccountNumber, decimal Amount, string TransactionPin)
        {
            ResponseDTO response = new ResponseDTO();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            var authUser = _accountService.Authenticate(AccountNumber, TransactionPin);
            if (authUser == null)
                throw new ApplicationException("Invalid credentials");

            try
            {
                sourceAccount = _accountService.GetByAccountNumber(AccountNumber);
                destinationAccount = _accountService.GetByAccountNumber(_ourBankSettlementAccount);

                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                if ((_dbContext.Entry(sourceAccount).State == EntityState.Modified) &&
                    (_dbContext.Entry(destinationAccount).State == EntityState.Modified))
                {
                    // Successfull
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction Successfull!";
                    response.Data = null;
                }
                else
                {
                    // Fail
                    transaction.TransactionStatus = TranStatus.Failed;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction failed`!";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"AN ERROR OCCURRED... => { ex.Message}");
            }

            transaction.TransactionType = TranType.Withdrawal;
            transaction.TransactionSourceAccount = AccountNumber;
            transaction.TransactionDestinationAccount = _ourBankSettlementAccount; 
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"NEW TRANSACTION " +
                $"FROM SOURCE => {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} " +
                $" TO DESTINATION ACCOUNT => {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} " +
                $" ON DATE {transaction.TransactionDate} FOR AMOUNT => {JsonConvert.SerializeObject(transaction.TransactionAmount)} " +
                $" TRANSACTION TYPE => {JsonConvert.SerializeObject(transaction.TransactionType)}" +
                $" TRANSACTION STATUS => {JsonConvert.SerializeObject(transaction.TransactionStatus)}";

            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();

            return response;
        }
    }
}
