using Microsoft.EntityFrameworkCore;
using PickGo_backend.Context;
using PickGo_backend.Models;
using PickGo_backend.Repositries;


namespace PickGo_backend.Repositories
{
    public class CourierTransactionRepository : BaseRepository<CourierTransaction>
        {
            public CourierTransactionRepository(DelieveryAppContext context) : base(context) { }


       

        }
    }


