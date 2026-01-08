using PickGo_backend.Context;
using PickGo_backend.Models.Lynx;
using Microsoft.EntityFrameworkCore;
using PickGo_backend.Repositries; // Note: Typo in namespace exists in SupplierRepository?
// Let's check SupplierRepository again or BaseRepository
// SupplierRepository line 3: using PickGo_backend.Repositries;
namespace PickGo_backend.Repositories
{
    public class AssignmentObservationRepository : BaseRepository<AssignmentObservation>
    {
        public AssignmentObservationRepository(DelieveryAppContext context) : base(context) { }

        public async Task<AssignmentObservation?> GetLatestForRequest(int requestId)
        {
            return await _table
                .Where(x => x.RequestId == requestId)
                .OrderByDescending(x => x.Timestamp)
                .FirstOrDefaultAsync();
        }
    }
}
