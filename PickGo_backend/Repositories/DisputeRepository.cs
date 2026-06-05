using Microsoft.EntityFrameworkCore;
using PickGo_backend.Context;
using PickGo_backend.Models;
using PickGo_backend.Repositries;

namespace PickGo_backend.Repositories
{
    public class DisputeRepository : BaseRepository<Dispute>
        {
            public DisputeRepository(DelieveryAppContext context) : base(context) { }


        public async Task<Dispute> GetDisputeDetailsAsync(int id)
        {
            return await _context.Disputes
                                 .Include(d => d.ProofImages)
                                 .Include(d => d.StatusHistory)
                                 .Include(d => d.Package)
                                 .FirstOrDefaultAsync(d => d.Id == id);
        }

    }

}
