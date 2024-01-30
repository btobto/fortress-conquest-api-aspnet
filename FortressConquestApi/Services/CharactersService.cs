using FortressConquestApi.Data;
using FortressConquestApi.DTOs;
using FortressConquestApi.Models;

namespace FortressConquestApi.Services
{
    public class CharactersService
    {
        private readonly FortressConquestContext _context;

        public CharactersService(FortressConquestContext context)
        {
            _context = context;
        }

        public async Task<Character?> GetCharacter(Guid id)
        {
            return await _context.Characters.FindAsync(id);
        }

        public CharacterDTO[] GetCharacterClasses()
        {
            return CharacterClassesService.GetCharacterClasses();
        }
    }
}
