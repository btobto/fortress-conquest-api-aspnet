using FortressConquestApi.DTOs;

namespace FortressConquestApi.Services
{
    public static class CharacterClassesService
    {
        private static CharacterDTO[] _classes = [
            new CharacterDTO
            {
                Name = "Assassin",
                Accuracy = 80,
                Armor = 10,
                CritChance = 30,
                Damage = 20,
                Health = 240,
            },
            new CharacterDTO
            {
                Name = "Barbarian",
                Accuracy = 65,
                Armor = 30,
                CritChance = 10,
                Damage = 20,
                Health = 360,
            },
            new CharacterDTO
            {
                Name = "Knight",
                Accuracy = 70,
                Armor = 29,
                CritChance = 15,
                Damage = 30,
                Health = 280,
            },
        ];

        public static CharacterDTO[] GetCharacterClasses()
        {
            return _classes;
        }

        public static CharacterDTO? GetCharacterClass(string name)
        {
            return _classes.FirstOrDefault(c => c.Name == name);
        }
    }
}
