using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityApp.Services
{
    public class JwtToken : IJwtToken
    {
        public string GenerateToken(string email, string otp, bool isPersistent)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("hdisgdgqgdgqlidblqgwdigqbiudbiuqdbgdq98dt96q8rgdtwq76grd87qw5gd87q578dq57dg587q5d78q5d78q5d87q5d87qd78d");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim("isPersistent", isPersistent.ToString())
                }),

                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("hdisgdgqgdgqlidblqgwdigqbiudbiuqdbgdq98dt96q8rgdtwq76grd87qw5gd87q578dq57dg587q5d78q5d78q5d87q5d87qd78d");

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
