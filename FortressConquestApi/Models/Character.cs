using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FortressConquestApi.Models
{
    public class Character
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public int Damage { get; set; }

        [Required]
        public int Armor { get; set; }

        [Required]
        public int Health { get; set; }

        [Required]
        public int Accuracy { get; set; }

        [Required]
        public int CritChance { get; set; }
    }
}
