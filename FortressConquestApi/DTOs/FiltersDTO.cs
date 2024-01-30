using FortressConquestApi.Models;

namespace FortressConquestApi.DTOs
{
    public class FiltersDTO
    {
        public double RadiusInM { get; set; }
        public int LevelFrom { get; set; }
        public int LevelTo { get; set; }
        public Location Location { get; set; } = null!;
    }
}
