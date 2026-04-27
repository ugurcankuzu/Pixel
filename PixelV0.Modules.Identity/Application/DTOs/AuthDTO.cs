namespace PixelV0.Modules.Identity.Application.DTOs
{
    public record RegisterRequest(
        string Username,
        string Email,
        string Password,
        string PasswordConfirmation
    );
    public record LoginRequest(
        string Username,
        string Password
    );
    public record AuthResponse(
        string Token,
        string Username,
        string Email,
        string Role
    );
}
