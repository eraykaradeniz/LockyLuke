using LockyLuke.AuthAPI.Entities.Users;
using SharedLibrary.Dtos.User;
using SharedLibrary.Dtos;

namespace LockyLuke.AuthAPI.Services
{
    public interface IUserService
    {
        Task<ResponseDto<AppUserDto>> CreateUserAsync(CreateUserDto createUserDto);

        Task<ResponseDto<AppUserDto>> GetUserByIdAsync(string id);

        Task<ResponseDto<AppUserDto>> GetUserByEmailAsync(string email);

        Task<ResponseDto<AppUserDto>> GetUserByUserNameAsync(string userName);
    }
}
