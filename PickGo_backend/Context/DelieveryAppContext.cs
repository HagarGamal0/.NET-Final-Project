using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PickGo_backend.Models;


namespace PickGo_backend.Context
{


    public class DelieveryAppContext : IdentityDbContext<ApplicationUser>
    {
        public DelieveryAppContext(DbContextOptions<DelieveryAppContext> options)
                   : base(options)
        {
        }


    
    }
}

