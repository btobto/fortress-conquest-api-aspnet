namespace FortressConquestApi.Models
{
    public class FortressDTO
    {
        public Guid Id { get; set; }

        public Guid OwnerId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime OwnedSince { get; set; }

        public long Latitude { get; set; }

        public long Longitude { get; set; }
    }
}
