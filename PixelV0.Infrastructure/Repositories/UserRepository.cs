using Microsoft.EntityFrameworkCore;        
using PixelV0.Modules.Identity.Application.Interfaces;
using PixelV0.Modules.Identity.Domain.Entity;
using PixelV0.Infrastructure.Data;

namespace PixelV0.Infrastructure.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        async Task IUserRepository.DeleteAsync(Guid UserID)
        {
            await context.Users.AsQueryable().Where(u => u.Id == UserID).ExecuteDeleteAsync();
        }

        async Task<bool> IUserRepository.ExistsByUsernameAsync(string username)
        {
            return await context.Users.AsNoTracking().Where(u => u.Username == username).AnyAsync();
        }

        async Task<bool> IUserRepository.ExistsByEmailAsync(string email)
        {
            return await context.Users.AsNoTracking().Where(u => u.Email == email).AnyAsync();
        }

        async Task<User?> IUserRepository.GetByEmailAsync(string email)
        {
            return await context.Users.AsNoTracking().Where(u => u.Email == email).FirstOrDefaultAsync();
        }

        async Task<User?> IUserRepository.GetByIDAsync(Guid UserID)
        {
            return await context.Users.AsNoTracking().Where(u => u.Id == UserID).FirstOrDefaultAsync();
        }

        async Task<User> IUserRepository.InsertAsync(User user)
        {
            await context.Users.AddAsync(user);
            return user;
        }

        async Task IUserRepository.SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
        async Task<User?> IUserRepository.GetByUsernameAsync(string username)
        {
            return await context.Users.AsNoTracking().Where(u => u.Username == username).FirstOrDefaultAsync();
        }

        async Task<User> IUserRepository.UpdateAsync(User user)
        {
            await context.Users.AsQueryable().Where(u => u.Id == user.Id).ExecuteUpdateAsync(u => u
                .SetProperty(p => p.Username, user.Username)
                .SetProperty(p => p.Email, user.Email)
                .SetProperty(p => p.PasswordHash, user.PasswordHash)
                .SetProperty(p => p.Role, user.Role)
                .SetProperty(p => p.AvatarUrl, user.AvatarUrl)
                .SetProperty(p => p.UpdatedAt, DateTime.UtcNow)
            );
            return user;
        }

        
    }
}
