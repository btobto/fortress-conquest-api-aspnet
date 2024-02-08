using FortressConquestApi.DTOs;
using FortressConquestApi.Models;
using FortressConquestApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FortressConquestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FortressesController : ControllerBase
    {
        private readonly FortressesService _fortressesService;

        public FortressesController(FortressesService fortressesService)
        {
            _fortressesService = fortressesService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Fortress>> GetFortress(Guid id)
        {
            var fortress = await _fortressesService.GetFortress(id);

            if (fortress == null)
            {
                return NotFound();
            }

            return fortress;
        }

        [HttpPost("search")]
        public async Task<ActionResult<List<Fortress>>> GetFiltered([FromBody] FiltersDTO filters)
        {
            return await _fortressesService.GetFiltered(filters);
        }

        [HttpPost]
        public async Task<ActionResult<Fortress>> SetFortress([FromBody] SetFortressDTO dto)
        {
            return await _fortressesService.SetFortress(dto);
        }
    }
}
