using System;
using System.ComponentModel.DataAnnotations;

namespace CoreBanking.API.Models
{
    public class UpdateAccountDTO
    {
        [Key]
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]/d{4}$", ErrorMessage = "Pin must not be more than 4 digits")] // 4-digit string
        public string Pin { get; set; }
        //[Required]
        //[Compare("Pin", ErrorMessage = "Pins do not match")]
        //public string ConfirmPin { get; set; }
        public DateTime DateLastUpdated { get; set; }
    }
}
