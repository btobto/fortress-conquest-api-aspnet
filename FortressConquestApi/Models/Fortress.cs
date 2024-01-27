using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations.Schema;

namespace FortressConquestApi.Models
{
    public class Fortress
    {
        public int Id { get; set; }

        public User Creator { get; set; } = null!;

        public User Owner { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime OwnedSince { get; set; }

        public Point Location { get; set; } = null!;
    }
}
