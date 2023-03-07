using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebShop.Abastract;
using WebShop.Data.Entities.Identity;

namespace WebShop.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<UserEntity> _userManager;
        public JwtTokenService(IConfiguration config, UserManager<UserEntity> userManager)
        {
            _config = config;
            _userManager = userManager;
        }

        public async Task<string> CreateToken(UserEntity user)
        {
            IList<string> roles = await _userManager.GetRolesAsync(user);
            List<Claim> claims = new List<Claim>()
            {
                new Claim("name", user.UserName),
                new Claim("image", user.Image)
            };

            foreach (var claim in roles)
            {
                claims.Add(new Claim("roles", claim));
            }
            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<String>("JWTSecretKey")));
            var signinCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                signingCredentials: signinCredentials,
                expires: DateTime.Now.AddDays(10),
                claims: claims
            );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string tokenId)
        {
            string clientID = _config["GoogleAuthSettings:ClientId"];
            var setting = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>() { clientID }
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(tokenId, setting);
            return payload;
        }
    }
}
