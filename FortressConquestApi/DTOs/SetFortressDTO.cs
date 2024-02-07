using FortressConquestApi.Models;

namespace FortressConquestApi.DTOs
{
    public class SetFortressDTO
    {
        public Guid UserId { get; set; }
        public GeoLocation Location { get; set; } = null!;
    }
}
