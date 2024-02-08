using FortressConquestApi.Common;
using FortressConquestApi.Common.Exceptions;
using FortressConquestApi.Data;
using FortressConquestApi.DTOs;
using FortressConquestApi.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FortressConquestApi.Services
{
    public class FortressesService
    {
        private readonly FortressConquestContext _context;
        ILogger<FortressesService> _logger;

        public FortressesService(FortressConquestContext context, ILogger<FortressesService> logger) 
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Fortress?> GetFortress(Guid id)
        {
            return await _context.Fortresses.FindAsync(id);
        }

        public async Task<List<Fortress>> GetFiltered(FiltersDTO filters)
        {
            (GeoLocation p1, GeoLocation p2) = filters.Location.BoundingCoordinates(filters.RadiusInM);

            var boundedFortresses = await _context.Fortresses
                .AsNoTracking()
                .Where(f => filters.LevelFrom <= f.Owner.Level && f.Owner.Level <= filters.LevelTo)
                .Where(f => p1.Latitude <= f.Latitude && f.Latitude <= p2.Latitude && p1.Longitude <= f.Longitude && f.Longitude <= p2.Longitude)
                .ToListAsync();

            return boundedFortresses
                .Where(f => filters.Location.DistanceInMeters(new GeoLocation(f.Latitude, f.Longitude)) <= filters.RadiusInM)
                .ToList();
        }

        public async Task<Fortress> SetFortress(SetFortressDTO dto)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(dto.Location));

            var user = await _context.Users
                .AsNoTracking()
                .Include(u => u.FortressesCreated)
                .FirstOrDefaultAsync(u => u.Id == dto.UserId);

            if (user == null)
            {
                throw new ItemNotFoundException("User not found.");
            }

            if (user.FortressesCreated.Count == user.Level)
            {
                throw new FortressPlacementForbiddenException("Maximum level of fortresses reached. Level up before placing another one.");
            }

            var fortress = new Fortress
            {
                OwnerId = user.Id,
                CreatorId = user.Id,
                Latitude = dto.Location.Latitude,
                Longitude = dto.Location.Longitude
            };

            await _context.Fortresses.AddAsync(fortress);
            await _context.SaveChangesAsync();

            return fortress;
        }
    }
}
