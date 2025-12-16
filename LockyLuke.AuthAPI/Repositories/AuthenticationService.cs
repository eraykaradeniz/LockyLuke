using LockyLuke.AuthAPI.Configuration;
using LockyLuke.AuthAPI.Entities.Users;
using LockyLuke.AuthAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedLibrary.Dtos;
using SharedLibrary.Dtos.User;
using SharedLibrary.Dtos.Auth;

namespace LockyLuke.AuthAPI.Repositories
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IGenericService<UserRefreshToken> _userRefreshTokenService;
        public AuthenticationService(ITokenService tokenService, UserManager<AppUser> userManager, IOptions<List<Client>> optClients, IGenericService<UserRefreshToken> userRefreshTokenService)
        {
            _tokenService = tokenService;
            _clients = optClients.Value;
            _userManager = userManager;
            _userRefreshTokenService = userRefreshTokenService;
        }

        public ResponseDto<ClientTokenDto> CreateClientToken(ClientLoginDto clientLoginDto)
        {
            var client = _clients.SingleOrDefault(x => x.ClientId == clientLoginDto.ClientId && x.ClientSecret == clientLoginDto.ClientSecret);

            if (client == null)
            {
                return ResponseDto<ClientTokenDto>.Fail("Client ID or Secret not found", true, 404);
            }

            var token = _tokenService.CreateClientToken(client);

            return ResponseDto<ClientTokenDto>.Success(new ClientTokenDto
            {
                AccessToken = token.AccessToken,
                AccessTokenExpire = token.AccessTokenExpire
            }, 200);

        }

        public async Task<ResponseDto<TokenDto>> CreateTokenAsync(UserLoginDto loginDto)
        {
            if (loginDto == null) throw new ArgumentNullException(nameof(UserLoginDto));

            var user = await _userManager.FindByNameAsync(loginDto.UserName);

            if (user == null) return ResponseDto<TokenDto>.Fail("Username or password is not correct", true,400,"");

            if (!await _userManager.CheckPasswordAsync(user,loginDto.Password))
                return ResponseDto<TokenDto>.Fail("Username or password is not correct", true, 400,"");

            var token =  _tokenService.CreateToken(user);

            var userRefreshToken = await _userRefreshTokenService.Where(x => x.UserId == user.Id).SingleOrDefaultAsync();

            if (userRefreshToken == null)
            {
                await _userRefreshTokenService.AddAsync(new UserRefreshToken
                {
                    UserId = user.Id,
                    Token = token.refreshToken,
                    ExpireDate = token.accessTokenExpire
                });
            }
            else
            {
                userRefreshToken.Token = token.refreshToken;
                userRefreshToken.ExpireDate = token.refreshTokenExpire;
            }
            _userRefreshTokenService.Update(userRefreshToken);
            return ResponseDto<TokenDto>.Success(token,200);
        }

        public async Task<ResponseDto<TokenDto>> CreateTokenbyRefreshTokenAsync(string refreshToken)
        {
            var refreshTokenDetail = await _userRefreshTokenService.Where(x => x.Token == refreshToken).SingleOrDefaultAsync();

            if (refreshTokenDetail == null)
                return ResponseDto<TokenDto>.Fail("RefreshToken not found", true, 404);
            
            var userDetail = await _userManager.FindByIdAsync(refreshTokenDetail.UserId);

            if(userDetail == null)
                return ResponseDto<TokenDto>.Fail("User not found", true, 404);

            var tokenDto = _tokenService.CreateToken(userDetail);

            refreshTokenDetail.Token = tokenDto.refreshToken;
            refreshTokenDetail.ExpireDate = tokenDto.refreshTokenExpire;

            _userRefreshTokenService.Update(refreshTokenDetail);

            return ResponseDto<TokenDto>.Success(tokenDto, 200);
        }

        public async Task<ResponseDto<NoDataDto>> RevokeRefreshTokenAsync(string refreshToken)
        {
            var refreshTokenDetail = await _userRefreshTokenService.Where(x => x.Token == refreshToken).SingleOrDefaultAsync();

            if (refreshTokenDetail == null)
                return ResponseDto<NoDataDto>.Fail("RefreshToken not found", true, 404);

            _userRefreshTokenService.Remove(refreshTokenDetail);

            return ResponseDto<NoDataDto>.Success(200);


        }
    }
}
