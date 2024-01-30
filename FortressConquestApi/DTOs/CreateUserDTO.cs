namespace FortressConquestApi.DTOs
{
    public class CreateUserDTO
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string PhotoUri { get; set; } = null!;

        public string CharacterName { get; set; } = null!;  
    }
}
