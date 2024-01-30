using FortressConquestApi.Data;
using FortressConquestApi.DTOs;
using FortressConquestApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FortressConquestApi.Services
{
    public class FortressesService
    {
        private readonly FortressConquestContext _context;

        public FortressesService(FortressConquestContext context) 
        {
            _context = context;
        }

        public async Task<Fortress?> GetFortress(Guid id)
        {
            return await _context.Fortresses.FindAsync(id);
        }

        public async Task<List<Fortress>> GetFiltered(FiltersDTO filters)
        {
            return await _context.Fortresses
                .Where(f => filters.LevelFrom <= f.Owner.Level && f.Owner.Level <= filters.LevelTo)
                .Where(f => filters.Location.DistanceInMeters(
                    new Location(f.Latitude, f.Longitude)) <= filters.RadiusInM)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Fortress> SetFortress(SetFortressDTO dto)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Include(u => u.FortressesCreated)
                .FirstOrDefaultAsync(u => u.Id == dto.UserId);

            if (user == null)
            {
                throw new NotImplementedException();
            }

            if (user.FortressesCreated.Count == user.Level)
            {
                throw new NotImplementedException();
            }

            // create

            throw new NotImplementedException();
        }
    }
}
