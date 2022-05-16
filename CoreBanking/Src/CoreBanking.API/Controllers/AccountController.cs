using AutoMapper;
using CoreBanking.API.Models;
using CoreBanking.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CoreBanking.API.Controllers
{
    [ApiController]
    [Route("api/v3/[controller]")]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;
        IMapper _mapper;
        public AccountController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("register_new_account")]
        public IActionResult RegisterNewAccount([FromBody] RegisterNewAccountDTO newAccount)
        {
            if(!ModelState.IsValid) return BadRequest(newAccount);

            // map to account
            var account = _mapper.Map<Account>(newAccount);
            return Ok(_accountService.Create(account, newAccount.Pin, newAccount.ConfirmPin));
        }

        [HttpGet]
        [Route("get_all_accounts")]
        public IActionResult GetAllAccounts()
        {
            // map Accunt Model to GetAccountModel
            var accounts = _accountService.GetAllAccounts();
            var cleanedAccounts = _mapper.Map<IList<GetAccountDTO>>(accounts);
            return Ok(cleanedAccounts);
        }

        [HttpPost]
        [Route("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateDTO model)
        {
            if (!ModelState.IsValid) return BadRequest(model);

            return Ok(_accountService.Authenticate(model.AccountNumber, model.Pin));
        }

        [HttpGet]
        [Route("get_by_account_number")]
        public IActionResult GetByAccountNumber(string AccountNumber)
        {
            if (!Regex.IsMatch(AccountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$"))
                return BadRequest("Account Number must be 10-digit");
            var account = _accountService.GetByAccountNumber(AccountNumber);
            var cleanedAccount = _mapper.Map<GetAccountDTO>(account);
            return Ok(cleanedAccount);
        }

        [HttpGet]
        [Route("get_account_by_id")]
        public IActionResult GetAccountById(int Id)
        {
            
            var account = _accountService.GetById(Id);
            var cleanedAccount = _mapper.Map<GetAccountDTO>(account);
            return Ok(cleanedAccount);
        }

        [HttpPut]
        [Route("update_account")]
        public IActionResult UpdateAccount([FromBody] UpdateAccountDTO model)
        {
            if(!ModelState.IsValid) 
                return BadRequest(model);
            
            var account = _mapper.Map<Account>(model);
            _accountService.Update(account, model.Pin);
            return Ok();
        }

    }
}
