namespace FortressConquestApi.DTOs
{
    public class BattleResultDTO
    {
        public Guid WinnerId { get; set; }
        public Guid LoserId { get; set; }
        public Guid FortressId { get; set; }
    }
}
