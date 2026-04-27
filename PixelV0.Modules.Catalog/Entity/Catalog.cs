using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelV0.Modules.Catalog.Entity
{
    public class Catalog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Game_Name { get; set; }
        public string Publisher { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
