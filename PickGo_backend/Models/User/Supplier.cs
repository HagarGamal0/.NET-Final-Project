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
        public virtual User User { get; set; }
        public bool IsDeleted { get; set; }


    }
}
