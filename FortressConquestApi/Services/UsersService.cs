using AutoMapper;
using FortressConquestApi.Common;
using FortressConquestApi.Data;
using FortressConquestApi.DTOs;
using FortressConquestApi.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection;

namespace FortressConquestApi.Services
{
    public class UsersService
    {
        private readonly FortressConquestContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersService> _logger;

        public UsersService(
            FortressConquestContext context,
            IMapper mapper,
            ILogger<UsersService> logger) 
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PaginatedList<User>> GetUsersSortedByXP(int page, int take)
        {
            return await PaginatedList<User>.CreateAsync(
                _context.Users.OrderByDescending(u => u.XP).AsNoTracking(), page, take);
        }

        public async Task<User?> GetUser(Guid id)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Include(u => u.Character)
                .Include(u => u.FortressesOwned)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user != null)
            {
                user.FortressCount = user.FortressesOwned.Count;
            }

            return user;
        }

        public async Task<User?> CreateUser(CreateUserDTO userDto)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(userDto));

            var characterDto = CharacterClassesService.GetCharacterClass(userDto.CharacterName);

            if (characterDto == null)
            {
                return null;
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            User? user = null;

            try
            {
                var character = _mapper.Map<Character>(characterDto);
                await _context.Characters.AddAsync(character);

                user = _mapper.Map<User>(userDto);
                user.Character = character;
                await _context.Users.AddAsync(user);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
            }

            return user;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task OnBattleWin(BattleResultDTO result)
        {
            var winner = await _context.Users.FindAsync(result.WinnerId);
            var loser = await _context.Users.FindAsync(result.LoserId);
            var fortress = await _context.Fortresses.FindAsync(result.FortressId);

            if (winner == null || loser == null || fortress == null) return;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var newXp = winner.XP + XPGained(winner, loser);
                var newLevel = newXp >= XPForNextLevel(winner.Level) ? winner.Level + 1 : winner.Level;

                winner.XP = newXp;
                winner.Level = newLevel;

                if (fortress.Owner.Id != winner.Id)
                {
                    fortress.Owner.Id = winner.Id;
                    fortress.OwnedSince = DateTime.Now;
                }

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
            }
        }

        private int XPGained(User winner, User loser)
        {
            var modifier = loser.Level - winner.Level;
            return modifier > 0 
                ? Constants.BaseXPGain + modifier * Constants.BaseXPModifier : Constants.BaseXPGain;
        }

        private int XPForNextLevel(int currentLevel)
        {
            return (int)Math.Floor(Constants.BaseXP * Math.Pow(currentLevel, Constants.LevelModifier));
        }

        public bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
