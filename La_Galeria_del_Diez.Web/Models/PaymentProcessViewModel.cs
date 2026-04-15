using System.ComponentModel.DataAnnotations;

namespace La_Galeria_del_Diez.Web.Models
{
    public class PaymentProcessViewModel
    {
        [Required]
        public int AuctionId { get; set; }

        public string AuctionName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cardholder name is required.")]
        [StringLength(100, ErrorMessage = "Cardholder name cannot exceed 100 characters.")]
        public string CardholderName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Card number is required.")]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Card number must contain exactly 16 digits.")]
        public string CardNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Expiration month is required.")]
        [Range(1, 12, ErrorMessage = "Expiration month must be between 1 and 12.")]
        public int ExpirationMonth { get; set; }

        [Required(ErrorMessage = "Expiration year is required.")]
        [Range(2024, 2100, ErrorMessage = "Expiration year is not valid.")]
        public int ExpirationYear { get; set; }

        [Required(ErrorMessage = "CVV is required.")]
        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "CVV must contain 3 or 4 digits.")]
        public string Cvv { get; set; } = string.Empty;
    }
}
