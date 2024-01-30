using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FortressConquestApi.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]

        public string LastName { get; set; } = null!;

        [Required]
        [EmailAddress]

        public string Email { get; set; } = null!;

        [Required]

        public string Password { get; set; } = null!;

        [Url]
        public string PhotoUri { get; set; } = null!;

        public Guid CharacterId { get; set; }

        [Required]
        public Character Character { get; set; } = null!;

        public int XP { get; set; }

        public int Level { get; set; }

        [JsonIgnore]
        [InverseProperty(nameof(Fortress.Creator))]
        public ICollection<Fortress> FortressesCreated { get; set; } = null!;

        [JsonIgnore]
        [InverseProperty(nameof(Fortress.Owner))]
        public ICollection<Fortress> FortressesOwned { get; set; } = null!;

        [NotMapped]
        public int FortressCount { get; set; }
    }
}
