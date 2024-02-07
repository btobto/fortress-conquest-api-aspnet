namespace FortressConquestApi.DTOs
{
    public class UserItemDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhotoUri { get; set; } = null!;
        public string CharacterId { get; set; } = null!;
        public int XP { get; set; }
        public int Level { get; set; }
    }
}
