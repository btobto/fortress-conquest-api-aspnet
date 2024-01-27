using FortressConquestApi.Data;
using FortressConquestApi.Models;
using NetTopologySuite.Geometries;

namespace FortressConquestApi.Services
{
    public class FortressesService
    {
        private readonly FortressConquestContext _context;

        public FortressesService(FortressConquestContext context) 
        {
            _context = context;
        }

        public async Task<Fortress?> GetFortress(int id)
        {
            return await _context.Fortresses.FindAsync(id);
        }
    }
}
