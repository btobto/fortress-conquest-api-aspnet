using System.ComponentModel.DataAnnotations;

namespace FortressConquestApi.DTOs
{
    public class CreateUserDTO
    {
        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = null!;

        [Url]
        public string PhotoUri { get; set; } = null!;

        [Required]
        public string CharacterName { get; set; } = null!;  
    }
}
