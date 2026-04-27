using PixelV0.Modules.Identity.Application.DTOs;
using PixelV0.Modules.Identity.Application.Interfaces;
using PixelV0.Modules.Identity.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelV0.Modules.Identity.Application.Services
{
    public class AuthService(IUserRepository user,IPasswordHasher hasher,IJWTProvider jwtProvider) : IAuthService
    {
        async Task<AuthResponse> IAuthService.LoginAsync(LoginRequest request)
        {
            try
            {
                User? existingUser = await user.GetByUsernameAsync(request.Username) ?? throw new Exception("User not found"); // TODO: Create custom exception for user not found
                bool isPasswordValid = hasher.Verify(request.Password, existingUser.PasswordHash);
                if (!isPasswordValid)
                {
                    throw new Exception("Invalid password"); // TODO: Create custom exception for invalid password
                }

                string token = jwtProvider.GenerateToken(existingUser);
                return new AuthResponse(token, existingUser.Username, existingUser.Email, existingUser.Role);

            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if needed
                throw new Exception("An error occurred during login. Please try again later.");
            }
        }
        async Task<AuthResponse> IAuthService.RegisterAsync(RegisterRequest request)
        {
            try
            {
                //Check if user already exists
                User? existingUser = await user.GetByUsernameAsync(request.Username);
                if (existingUser != null)
                {
                    throw new Exception("User already exists"); // TODO: Create custom exception for user already exists
                }
                //Check Email already exists
                existingUser = await user.GetByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    throw new Exception("Email already exists"); // TODO: Create custom exception for email already exists
                }
                //Check if passwords match
                if (request.Password != request.PasswordConfirmation)
                {
                    throw new Exception("Passwords do not match"); // TODO: Create custom exception for passwords do not match
                }
                //Create new user
                User newUser = new User
                {
                    Id = Guid.NewGuid(),
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = hasher.Hash(request.Password),
                    Role = "User", // Default role, can be changed later
                    AvatarUrl = "", // Default avatar, can be changed later
                    CreatedAt = DateTime.UtcNow
                };
                await user.InsertAsync(newUser);
                await user.SaveChangesAsync();
                string token = jwtProvider.GenerateToken(newUser);

                return new AuthResponse(token, newUser.Username, newUser.Email, newUser.Role);
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if needed
                throw new Exception("An error occurred during registration. Please try again later.");
            }
        }
    }
}
