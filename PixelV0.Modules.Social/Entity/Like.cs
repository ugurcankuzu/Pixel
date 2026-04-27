using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelV0.Modules.Social.Entity
{
    public class Like
    {
        public Guid User_ID { get; set; }
        public Guid Post_ID { get; set; }
    }
}
