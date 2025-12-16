using LockyLuke.AuthAPI.Configuration;
using SharedLibrary.Dtos.Auth;
using LockyLuke.AuthAPI.Entities.Users;

namespace LockyLuke.AuthAPI.Services
{
    public interface ITokenService
    {
        TokenDto CreateToken(AppUser appUser);
        ClientTokenDto CreateClientToken(Client client);
    }
}
