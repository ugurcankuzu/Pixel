using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelV0.Modules.Social.Entity
{
    public class Post
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid User_ID { get; set; }
        public Guid Game_ID { get; set; }
        public Guid Preset_ID { get; set; }
        public string Image_URL { get; set; } = "";
        public string Caption { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
