using LockyLuke.AuthAPI.Configuration;
using LockyLuke.AuthAPI.Entities.Users;
using LockyLuke.AuthAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using SharedLibrary.Services;
using SharedLibrary.Dtos.Auth;
namespace LockyLuke.AuthAPI.Repositories
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly CustomTokenOption _tokenOptions;
        public TokenService(UserManager<AppUser> userManager, IOptions<CustomTokenOption> options)
        {
            _userManager = userManager;
            _tokenOptions = options.Value;
        }

        private IEnumerable<Claim> GetClaims(AppUser appUser, List<string> audiences)
        {
            var userList = new List<Claim>
          {
              new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
              new Claim("userId", appUser.Id.ToString()),
              new Claim(ClaimTypes.Name, appUser.UserName),
              new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
          };

            userList.AddRange(audiences.Select(audience => new Claim(JwtRegisteredClaimNames.Aud, audience)));
            return userList;
        }

        private IEnumerable<Claim> GetClaimsbyClient(Client client)
        {

            var claims = new List<Claim>();
            claims.AddRange(client.Audiences.Select(audience => new Claim(JwtRegisteredClaimNames.Aud, audience)));

            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, client.ClientId.ToString()));

            return claims;
        }

        private string CreateRefreshToken()
        {
            var numberByte = new byte[32];
            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(numberByte);

            return Convert.ToBase64String(numberByte);
        }

        public ClientTokenDto CreateClientToken(Client client)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
            var securityKey = SignService.GetSecurityKey(_tokenOptions.SecurityKey);
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwttoken = new(
               issuer: _tokenOptions.Issuer,
               expires: accessTokenExpiration,
               notBefore: DateTime.Now,
               claims: GetClaimsbyClient(client),
               signingCredentials: signingCredentials);

            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwttoken);

            var clientTokenDto = new ClientTokenDto
            {
                AccessToken = token,
                AccessTokenExpire = accessTokenExpiration
            };

            return clientTokenDto;
        }

        public TokenDto CreateToken(AppUser appUser)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);

            var securityKey = SignService.GetSecurityKey(_tokenOptions.SecurityKey);

            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwttoken = new(
                issuer: _tokenOptions.Issuer,
                expires: accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: GetClaims(appUser, _tokenOptions.Audience),
                signingCredentials: signingCredentials);

            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwttoken);

            var tokenDto = new TokenDto
            {
                accessToken = token,
                accessTokenExpire = accessTokenExpiration,
                refreshToken = CreateRefreshToken(),
                refreshTokenExpire = DateTime.Now.AddMinutes(_tokenOptions.RefreshTokenExpiration),
                userId = appUser.Id 
            };

            return tokenDto;
        }
    }
}
