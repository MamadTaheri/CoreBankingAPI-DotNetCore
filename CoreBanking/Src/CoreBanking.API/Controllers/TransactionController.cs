using AutoMapper;
using CoreBanking.API.Models;
using CoreBanking.API.Models.DTOs;
using CoreBanking.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace CoreBanking.API.Controllers
{
    [ApiController]
    [Route("api/v2/[controller]")]
    public class TransactionController : ControllerBase
    {
        private ITransactionService _transactionService;
        IMapper _mapper;
        public TransactionController(ITransactionService transactionService, IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("create_new_transaction")]
        public IActionResult CreateNewTransaction([FromBody] TransactionRequestDTO transactionRequest)
        {
            // we sholud use an dto instead of Main Transaction Model
            // ALso, Map Request dto to transaction
            if(!ModelState.IsValid)
                return BadRequest(transactionRequest);
            var transaction = _mapper.Map<Transaction>(transactionRequest);
            return Ok(_transactionService.CreateNewTransaction(transaction));
        }

        [HttpPost]
        [Route("make_deposit")]
        public IActionResult MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin)
        {
            if (!Regex.IsMatch(AccountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$"))
                return BadRequest("Account Number must be 10-digit");
            return Ok(_transactionService.MakeDeposit(AccountNumber, Amount, TransactionPin));
        }

        [HttpPost]
        [Route("make_withdrawal")]
        public IActionResult MakeWithDrawal(string AccountNumber, decimal Amount, string TransactionPin)
        {
            if (!Regex.IsMatch(AccountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$"))
                return BadRequest("Account Number must be 10-digit");
            return Ok(_transactionService.MakeWithDrawal(AccountNumber, Amount, TransactionPin));
        }

        [HttpPost]
        [Route("make_fund_transfer")]
        public IActionResult MakeFundsTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
        {
            if (!Regex.IsMatch(FromAccount, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$") || !Regex.IsMatch(ToAccount, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$"))
                return BadRequest("Account Number must be 10-digit");
            return Ok(_transactionService.MakeFundsTransfer(FromAccount, ToAccount, Amount, TransactionPin));
        }

    }
}
