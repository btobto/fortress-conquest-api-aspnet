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

namespace FortressConquestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly FortressConquestContext _context;
        private readonly UsersService _usersService;

        public UsersController(FortressConquestContext context, UsersService usersService)
        {
            _context = context;
            _usersService = usersService;
        }

        [HttpGet("leaderboard")]
        public async Task<ActionResult<PaginatedList<User>>> GetUsers([FromQuery] int page, [FromQuery] int  take)
        {
            return await _usersService.GetUsersSortedByXP(page, take);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _usersService.GetUser(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            try
            {
                await _usersService.PutUser(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_usersService.UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            await _usersService.CreateUser(user);

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
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

        }
    }
}
