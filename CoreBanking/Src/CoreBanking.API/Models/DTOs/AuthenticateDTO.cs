using System.ComponentModel.DataAnnotations;

namespace CoreBanking.API.Models
{
    public class AuthenticateDTO
    {
        [Required]
        [RegularExpression(@"^[0][1-9]\d{9}$|^[1-9]\d{9}$")]
        public string AccountNumber { get; set; } // -> Should be 10-digit
        [Required]
        public string Pin { get; set; }
    }
}
