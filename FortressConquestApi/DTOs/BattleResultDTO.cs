using System.ComponentModel.DataAnnotations;

namespace FortressConquestApi.DTOs
{
    public class BattleResultDTO
    {
        [Required]
        public Guid WinnerId { get; set; }

        [Required]
        public Guid LoserId { get; set; }

        [Required]
        public Guid FortressId { get; set; }
    }
}
