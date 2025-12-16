using LockyLuke.AuthAPI.Configuration.DtoMapper;
using SharedLibrary.Dtos.User;
using LockyLuke.AuthAPI.Entities.Users;
using LockyLuke.AuthAPI.Services;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.Dtos;

namespace LockyLuke.AuthAPI.Repositories
{
    public class UserService : IUserService
    {

        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ResponseDto<AppUserDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            AppUser userDetail = new()
            {
                Email = createUserDto.Email,
                UserName = createUserDto.UserName
            };

            var result = await _userManager.CreateAsync(userDetail, createUserDto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();

                return ResponseDto<AppUserDto>.Fail(new ErrorDto(errors, true), 400);
            }

            return ResponseDto<AppUserDto>.Success(
                ObjectMapper.Mapper.Map<AppUserDto>(userDetail), 200
                );
        }

        public async Task<ResponseDto<AppUserDto>> GetUserByEmailAsync(string email)
        {
            var userDetail = await _userManager.FindByEmailAsync(email);

            if (userDetail == null)
                return ResponseDto<AppUserDto>.Fail("User not found",true, 404);

            return ResponseDto<AppUserDto>.Success(
             ObjectMapper.Mapper.Map<AppUserDto>(userDetail), 200
             );
        }

        public async Task<ResponseDto<AppUserDto>> GetUserByIdAsync(string id)
        {
            var userDetail = await _userManager.FindByIdAsync(id);

            if (userDetail == null)
                return ResponseDto<AppUserDto>.Fail("User not found", true, 404);

            return ResponseDto<AppUserDto>.Success(
             ObjectMapper.Mapper.Map<AppUserDto>(userDetail), 200
             );
        }

        public async Task<ResponseDto<AppUserDto>> GetUserByUserNameAsync(string userName)
        {
            var userDetail = await _userManager.FindByNameAsync(userName);

            if (userDetail == null)
                return ResponseDto<AppUserDto>.Fail("User not found", true, 404);

            return ResponseDto<AppUserDto>.Success(
             ObjectMapper.Mapper.Map<AppUserDto>(userDetail), 200
             );
        }
    }
}
