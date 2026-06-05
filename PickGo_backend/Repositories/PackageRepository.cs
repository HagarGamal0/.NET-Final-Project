using Microsoft.EntityFrameworkCore;
using PickGo_backend.Context;
using PickGo_backend.Models;
using PickGo_backend.Repositries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PickGo_backend.Repositories
{
    public class PackageRepository : BaseRepository<Package>
    {
        public PackageRepository(DelieveryAppContext context) : base(context) { }

        // دالة جلب كل الباكدجات المرتبطة بركوست معين
        public async Task<IEnumerable<Package>> GetByRequestIdAsync(int requestId)
        {
            return await _context.Packages
                                 .Where(p => p.RequestID == requestId)
                                 .ToListAsync();
        }

        // دالة لجلب كل الباكدجات مع الـ includes
        //public async Task<IEnumerable<Package>> GetAllIncludingAsync()
        //{
        //    return await _context.Packages
        //                         .Include(p => p.Request)
        //                         .Include(p => p.Customer)
        //                             .ThenInclude(c => c.User)
        //                         .ToListAsync();
        //}

        public async Task<IEnumerable<Package>> GetAllWithIncludesAsync()
        {
            return await _context.Packages
                .Include(p => p.Request)
                .Include(p => p.Customer)
                    .ThenInclude(c => c.User)
                .ToListAsync();
        }

    }
}
