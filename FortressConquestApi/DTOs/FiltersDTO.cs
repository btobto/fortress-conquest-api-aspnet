using FortressConquestApi.Models;
using System.ComponentModel.DataAnnotations;

namespace FortressConquestApi.DTOs
{
    public class FiltersDTO
    {
        public double RadiusInKm { get; set; }

        [Range(1, int.MaxValue)]
        public int LevelFrom { get; set; }

        [Range(1, int.MaxValue)]
        public int LevelTo { get; set; }

        [Required]
        public Location Location { get; set; } = null!;
    }
}
