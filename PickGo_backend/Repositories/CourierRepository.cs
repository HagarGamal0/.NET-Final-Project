using Microsoft.EntityFrameworkCore;
using PickGo_backend.Context;
using PickGo_backend.Models;
using PickGo_backend.Repositries;


namespace PickGo_backend.Repositories
{
    public class CourierRepository : BaseRepository<Courier>
        {
            public CourierRepository(DelieveryAppContext context) : base(context) { }


        public async Task<IEnumerable<Courier>> GetAllWithLocationsAsync()
        {
            return await _table
                .Include(c => c.Locations)   // Include for locations
                .ToListAsync();
        }


        public async Task<Courier?> GetByIdWithIncludesAsync(int id)
        {
            return await _context.Couriers
                .Include(c => c.CourierSubscriptions)
                .Include(c => c.CurrentSubscription)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Courier>> GetAllWithUserAsync()
{
    return await _context.Couriers
        .Include(c => c.User)
        .ToListAsync();
}

    }
}


