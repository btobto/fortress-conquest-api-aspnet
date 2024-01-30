using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FortressConquestApi.Models
{
    public class Fortress
    {
        public Guid Id { get; set; }

        public Guid CreatorId { get; set; }

        [JsonIgnore]
        [Required]
        public User Creator { get; set; } = null!;

        public Guid OwnerId { get; set; }

        [JsonIgnore]
        [Required]
        public User Owner { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime OwnedSince { get; set; }
        
        public long Latitude { get; set; }

        public long Longitude { get; set; }
    }
}
