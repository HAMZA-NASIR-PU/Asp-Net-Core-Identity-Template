using Microsoft.AspNetCore.Identity;

namespace IdentityApp.Models
{
    public class AppUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Otp { get; set; }
        public DateTime? OtpExpiration { get; set; }
        public int FailedOtpAttempts { get; set; }
        public DateTime? OtpLockoutEnd { get; set; }
    }
}
