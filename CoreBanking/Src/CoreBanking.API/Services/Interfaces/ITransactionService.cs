using CoreBanking.API.Models;
using System;

namespace CoreBanking.API.Services.Interfaces
{
    public interface ITransactionService
    {
        ResponseDTO CreateNewTransaction(Transaction transaction);
        ResponseDTO FindTransactionByDate(DateTime date);
        ResponseDTO MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin);
        ResponseDTO MakeWithDrawal(string AccountNumber, decimal Amount, string TransactionPin);
        ResponseDTO MakeFundsTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin);
    }
}
