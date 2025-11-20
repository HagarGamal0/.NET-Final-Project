using Microsoft.AspNetCore.Identity;
using PickGo_backend.Context;
using PickGo_backend.Repositries;

namespace PickGo_backend.Repositories
{
    public class RoleRepository : BaseRepository<IdentityRole>
    {
        public RoleRepository(DelieveryAppContext context) : base(context)
        {
        }
    }
}
