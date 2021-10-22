using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.Dtos
{
    public class LoginUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Password { get; set; }

    }
}
