using PixelV0.Modules.Identity.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelV0.Infrastructure.Authentication
{
    public class PasswordHasher : IPasswordHasher
    {
        string IPasswordHasher.Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        bool IPasswordHasher.Verify(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
