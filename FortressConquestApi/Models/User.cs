using System.ComponentModel.DataAnnotations.Schema;

namespace FortressConquestApi.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string PhotoUri { get; set; } = null!;

        public Character Character { get; set; } = null!;

        public int XP { get; set; }

        public int Level { get; set; }

        [InverseProperty(nameof(Fortress.Creator))]
        public ICollection<Fortress> FortressesCreated { get; set; } = null!;

        [InverseProperty(nameof(Fortress.Owner))]
        public ICollection<Fortress> FortressesOwned { get; set; } = null!;
    }
}
