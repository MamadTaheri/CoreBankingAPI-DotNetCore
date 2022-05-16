using System;
using System.ComponentModel.DataAnnotations;

namespace CoreBanking.API.Models
{
    public class RegisterNewAccountDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        // public string AccountName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        // public decimal CurrentAccountBalance { get; set; }
        public AccountType AccountType { get; set; }
        // public string AccountNumberGenerated { get; set; }
        // public byte[] PinHash { get; set; }
        // public byte[] PinSalt { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "پین وارد شده بایستی حداکثر 4 رقم باشد")] // 4-digit string
        public string Pin { get; set; }
        [Required]
        [Compare("Pin", ErrorMessage = "پین و تکرار پین بایستی برابر باشند ")]
        public string ConfirmPin { get; set; }
    }
}
