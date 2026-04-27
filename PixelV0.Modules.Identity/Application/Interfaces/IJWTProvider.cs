using PixelV0.Modules.Identity.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelV0.Modules.Identity.Application.Interfaces
{
    public interface IJWTProvider
    {
        string GenerateToken(User user);
    }
}
