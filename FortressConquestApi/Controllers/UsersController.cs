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
using Newtonsoft.Json;

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
        public async Task<ActionResult<PaginatedList<UserDTO>>> GetUsers([FromQuery] int page, [FromQuery] int  take)
        {
            var users = await _usersService.GetUsersSortedByXP(page, take);
            return _mapper.Map<PaginatedList<UserDTO>>(users);
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
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (!(await _usersService.DeleteUser(id)))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost("win")]
        public async Task<IActionResult> OnBattleWin([FromBody] BattleResultDTO result)
        {
            await _usersService.OnBattleWin(result);
            return NoContent();
        }

        [HttpPost("test")]
        public void Test([FromBody] CreateUserDTO dto)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(dto));
            var user = _mapper.Map<User>(dto);
            _logger.LogInformation(JsonConvert.SerializeObject(user));
        }
    }
}
