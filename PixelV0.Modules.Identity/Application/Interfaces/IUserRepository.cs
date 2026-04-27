using PixelV0.Modules.Identity.Domain.Entity;
using System;
using System.Threading.Tasks;

namespace PixelV0.Modules.Identity.Application.Interfaces
{
    public interface IUserRepository
    {
        // CRUD operations for User entity
        Task<User> InsertAsync(User user);
        Task<User?> GetByIDAsync(Guid UserID);
        Task<User> UpdateAsync(User user);
        Task DeleteAsync(Guid UserID);

        // Additional query methods
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username); // Unique Username kontrolü için eklendi
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByUsernameAsync(string username); // Unique Username kontrolü için eklendi
        Task SaveChangesAsync();
    }
}