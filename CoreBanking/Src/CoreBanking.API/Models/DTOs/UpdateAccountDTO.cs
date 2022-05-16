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
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "پین وارد شده بایستی حداکثر 4 رقم باشد")] // 4-digit string
        public string Pin { get; set; }
        
        [Required]
        [Compare("Pin", ErrorMessage = "پین و تکرار پین بایستی برابر باشند ")]
        public string ConfirmPin { get; set; }
        public DateTime DateLastUpdated { get; set; }
    }
}
