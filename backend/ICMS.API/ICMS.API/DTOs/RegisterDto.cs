using System.ComponentModel.DataAnnotations;

namespace ICMS.API.DTOs
{
    public class RegisterDto
    {
        [Required]
        [StringLength(100)]
        public required string FullName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(6)]
        public required string Password { get; set; }

        [Required]
        [RegularExpression("^(Student|Recruiter|Admin)$")]
        public required string Role { get; set; }
    }
}