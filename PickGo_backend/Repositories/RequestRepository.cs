using Microsoft.EntityFrameworkCore;
using PickGo_backend.Context;
using PickGo_backend.Models;
using PickGo_backend.Repositries;


namespace PickGo_backend.Repositories
{
    public class RequestRepository : BaseRepository<Request>
        {
            public RequestRepository(DelieveryAppContext context) : base(context) { }

            public async Task<Request?> GetWithPackagesAsync(int id)
        {
            return await _context.Requests
                                 .Include(r => r.Packages)
                                 .FirstOrDefaultAsync(r => r.Id == id);
        }
       
        public async Task<List<Request>> GetBySupplierAsync(int supplierId)
{
    return await _context.Requests
        .Where(r => r.SupplierId == supplierId)
        .Include(r => r.Packages)
        .ToListAsync();
}

public async Task<Request?> GetFullRequestAsync(int id)
{
    return await _context.Requests
        .Include(r => r.Packages)
        .FirstOrDefaultAsync(r => r.Id == id);
}


        }
    }


