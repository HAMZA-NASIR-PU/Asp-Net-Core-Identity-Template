namespace IdentityApp.Services
{
    public class OtpService : IOtpService
    {
        public string GenerateOtp()
        {
            var rng = new Random();
            return rng.Next(100000, 999999).ToString();
        }
    }
}
