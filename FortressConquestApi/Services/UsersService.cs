using FortressConquestApi.Common;
using FortressConquestApi.Data;
using FortressConquestApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FortressConquestApi.Services
{
    public class UsersService
    {
        private readonly FortressConquestContext _context;
        private readonly FortressesService _fortressesService;

        public UsersService(
            FortressConquestContext context,
            FortressesService fortressesService) 
        {
            _context = context;
            _fortressesService = fortressesService;
        }

        public async Task<PaginatedList<User>> GetUsersSortedByXP(int page, int take)
        {
            return await PaginatedList<User>.CreateAsync(
                _context.Users.OrderByDescending(u => u.XP).AsNoTracking(), page, take);
        }

        public async Task<User?> GetUser(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task PutUser(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
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
            var winner = await GetUser(result.WinnerId);
            var loser = await GetUser(result.LoserId);
            var fortress = await _fortressesService.GetFortress(result.FortressId);

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

        public bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
