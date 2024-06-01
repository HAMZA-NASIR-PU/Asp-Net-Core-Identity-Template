using System.Security.Claims;

namespace IdentityApp.Services
{
    public interface IJwtToken
    {
        string GenerateToken(string email, string otp, bool isPersistent);
        ClaimsPrincipal ValidateToken(string token);
    }
}
