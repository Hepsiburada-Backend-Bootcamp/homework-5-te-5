using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.Dtos
{
    public class RegisterUserDto
    {   [Required]
        [StringLength(56,MinimumLength = 2)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(56, MinimumLength = 2)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Password { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string ConfirmPassword { get; set; }
    }
}
