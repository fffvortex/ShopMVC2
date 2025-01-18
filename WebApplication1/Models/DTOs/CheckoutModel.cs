using System.ComponentModel.DataAnnotations;

namespace ShopMVC2.Models.DTOs
{
    public class CheckoutModel
    {
        [Required, MaxLength(30)]
        public string? Name { get; set; }

        [Required, EmailAddress, MaxLength(40)]
        public string? Email { get; set; }

        [Required]
        public string? MobileNumber { get; set; }
        [Required, MaxLength(200)]
        public string? Address { get; set; }

        [Required]
        public string? PaymentMethod { get; set; }
    }
}
