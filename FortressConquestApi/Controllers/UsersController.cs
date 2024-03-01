using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FortressConquestApi.Data;
using FortressConquestApi.Models;
using FortressConquestApi.Services;
using FortressConquestApi.DTOs;
using AutoMapper;

namespace FortressConquestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersService _usersService;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            UsersService usersService,
            IMapper mapper,
            ILogger<UsersController> logger)
        {
            _usersService = usersService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("leaderboard")]
        public async Task<ActionResult<PaginatedList<UserItemDTO>>> GetUsers([FromQuery] int page = 1, [FromQuery] int  pageSize = 10)
        {
            const int maxPageSize = 20;

            if (page < 1)
            {
                return BadRequest("Page number cannot be less that 1.");
            }

            if (pageSize < 1 || pageSize > maxPageSize)
            {
                return BadRequest($"Page size must be between 1 and {maxPageSize}.");
            }

            var users = await _usersService.GetUsersSortedByXP(page, pageSize);
            return _mapper.Map<PaginatedList<UserItemDTO>>(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(Guid id)
        {
            var user = await _usersService.GetUser(id);

            if (user == null)
            {
                return NotFound();
            }

            return _mapper.Map<UserDTO>(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(CreateUserDTO userDto)
        {
            var user = await _usersService.CreateUser(userDto);

            if (user == null)
            {
                return BadRequest();
            }

            return CreatedAtAction("GetUser", new { id = user.Id }, _mapper.Map<UserDTO>(user));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _usersService.DeleteUser(id);
            return NoContent();
        }

        [HttpPost("win")]
        public async Task<IActionResult> OnBattleWin([FromBody] BattleResultDTO result)
        {
            await _usersService.OnBattleWin(result);
            return NoContent();
        }
    }
}
