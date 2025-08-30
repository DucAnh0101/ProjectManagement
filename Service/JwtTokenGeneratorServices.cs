using DataAccessLayer.ResDTO;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service
{
    public class JwtTokenGeneratorServices
    {
        private readonly JWTSettings _settings;

        public JwtTokenGeneratorServices(IOptions<JWTSettings> settings)
        {
            _settings = settings.Value;
        }

        public string GenerateToken(LoginResDTO res)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, res.UserId.ToString()),
            new Claim(ClaimTypes.Name, res.Username),
            new Claim(ClaimTypes.Role, res.RoleSystem.ToString()),
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class JWTSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiryMinutes { get; set; }
    }
}