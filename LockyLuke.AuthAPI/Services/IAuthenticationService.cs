using SharedLibrary.Dtos.Auth;
using SharedLibrary.Dtos;
using SharedLibrary.Dtos.User;

namespace LockyLuke.AuthAPI.Services
{
    public interface IAuthenticationService
    {
        Task<ResponseDto<TokenDto>> CreateTokenAsync(UserLoginDto loginDto);

        Task<ResponseDto<TokenDto>> CreateTokenbyRefreshTokenAsync(string refreshToken);

        Task<ResponseDto<NoDataDto>> RevokeRefreshTokenAsync(string refreshToken);

        ResponseDto<ClientTokenDto> CreateClientToken(ClientLoginDto clientLoginDto);
    }
}
