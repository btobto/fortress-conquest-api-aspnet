using FortressConquestApi.Models;
using System.ComponentModel.DataAnnotations;

namespace FortressConquestApi.DTOs
{
    public class SetFortressDTO
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Location Location { get; set; } = null!;
    }
}
