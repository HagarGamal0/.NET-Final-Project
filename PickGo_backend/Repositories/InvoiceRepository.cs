using Microsoft.EntityFrameworkCore;
using PickGo_backend.Context;
using PickGo_backend.Models;
using PickGo_backend.Repositries;


namespace PickGo_backend.Repositories
{
    public class InvoiceRepository : BaseRepository<Invoice>
        {
            public InvoiceRepository(DelieveryAppContext context) : base(context) { }


       

        }
    }


