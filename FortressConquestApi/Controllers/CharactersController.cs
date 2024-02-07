using FortressConquestApi.DTOs;
using FortressConquestApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FortressConquestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharactersController
    {
        private CharactersService _charactersService;

        public CharactersController(CharactersService charactersService)
        {
            _charactersService = charactersService;
        }

        [HttpGet]
        public ActionResult<CharacterDTO[]> GetCharacters()
        {
            return _charactersService.GetCharacterClasses();
        }
    }
}
