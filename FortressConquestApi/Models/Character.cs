namespace FortressConquestApi.Models
{
    public class Character
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int Damage { get; set; }

        public int Armor { get; set; }

        public int Health { get; set; }

        public int Accuracy { get; set; }

        public int CritChance { get; set; }
    }
}
