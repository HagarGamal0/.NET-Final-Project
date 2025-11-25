using PickGo_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PickGo_backend.Models
{
    //is - a
    public class Supplier :BaseModel
    {
        public string UserId { get; set; } 
        public string ShopName { get; set; } = null!;
        public bool IsDeleted { get; set; }


        public virtual User? User { get; set; } 
        public virtual ICollection<Courier> Couriers { get; set; }
        public ICollection<Request>? Requests { get; set; } = new List<Request>();

    }
}
