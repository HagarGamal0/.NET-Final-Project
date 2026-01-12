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
        .Include(r => r.Packages)
        .Where(r => r.SupplierId == supplierId)
        .ToListAsync();
}





        public async Task<Request?> GetFullRequestAsync(int id)
        {
            return await _context.Requests
                .Include(r => r.Packages)
                .FirstOrDefaultAsync(r => r.Id == id);
        }




        public async Task<Request?> GetActiveRequestForCustomerAsync(int customerId)
        {
            return await _context.Requests
                .Where(r => r.SupplierId == customerId && r.Status == RequestStatus.Pending) // or Processing if you have it
                .OrderByDescending(r => r.CreatedAt)
                .FirstOrDefaultAsync();
        }




        public async Task<Request?> GetByIdWithPackagesAsync(int id)
        {
            return await _context.Requests
                .Include(r => r.Packages)
                .ThenInclude(p => p.Courier)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Request>> GetAllWithPackagesAsync()
        {
            return await _context.Requests
                .Include(r => r.Packages)
                .ThenInclude(p => p.Courier)
                .ThenInclude(c => c.User)
                .ToListAsync();
        }



    }

    }


