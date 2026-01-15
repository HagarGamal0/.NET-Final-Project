using Microsoft.EntityFrameworkCore;
using PickGo_backend.Context;
using PickGo_backend.Models;
using PickGo_backend.Repositries;

namespace PickGo_backend.Repositories
{
    public class SupplierRepository : BaseRepository<Supplier>
    {
        private readonly DelieveryAppContext _context;

        public SupplierRepository(DelieveryAppContext context) : base(context)
        {
            _context = context;
        }

        // 🔥 THIS IS THE FIX
        public async Task<Supplier?> GetSupplierWithIncludesAsync(string userId)
        {
            return await _context.Suppliers
                .Include(s => s.User)
                .Include(s => s.Requests)
                    .ThenInclude(r => r.Packages)
                        .ThenInclude(p => p.Customer)
                            .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(s => s.UserId == userId);
        }
    }
}
