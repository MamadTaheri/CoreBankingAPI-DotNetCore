using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CoreBanking.API.Models
{
    [Table("Accounts")]
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal CurrentAccountBalance { get; set; }
        public AccountType AccountType { get; set; }
        public string AccountNumberGenerated { get; set; }
        
        [JsonIgnore]
        public byte[] PinHash { get; set; }
        
        [JsonIgnore]
        public byte[] PinSalt { get; set; }
        
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }
        Random random = new Random();

        public Account()
        {
            // Generate Account Name
            AccountName = $"{FirstName} {LastName}";

            // Generate Account Number
            AccountNumberGenerated = Convert.ToString((long) Math.Floor(random.NextDouble() * 9_000_000_000L + 1_000_000_000L));
        }
    }

    public enum AccountType
    {
        Savings,
        Current,
        Corporate,
        Government
    }
}
