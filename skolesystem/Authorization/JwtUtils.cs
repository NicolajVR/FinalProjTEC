using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using skolesystem.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace skolesystem.Authorization
{
    // Grænseflade til at generere og validere JWT-token
    public interface IJwtUtils
    {
        // Genererer et JWT-token baseret på brugeroplysninger
        public string GenerateJwtToken(Users user);

        // Validerer et JWT-token og returnerer brugerens ID
        public int? ValidateJwtToken(string token);
    }

    // Implementering af IJwtUtils-grænsefladen
    public class JwtUtils : IJwtUtils
    {
        // AppSettings-instantiering til adgang til hemmelig nøgle
        private readonly AppSettings _appSettings;

        // Konstruktør, der modtager appSettings som en IOptions-injektion
        public JwtUtils(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        // Genererer et JWT-token baseret på brugeroplysninger
        public string GenerateJwtToken(Users user)
        {
            // Genererer et token, der er gyldigt i 7 dage
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.user_id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // Validerer et JWT-token og returnerer brugerens ID
        public int? ValidateJwtToken(string token)
        {
            if (token == null)
            {
                return null;
            }

            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;
                int userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                return userId;
            }
            catch (Exception ex)
            {
                // Håndter fejl under validering
                return null;
            }
        }
    }
}