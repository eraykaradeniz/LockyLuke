using AutoMapper;
using SharedLibrary.Dtos.User;
using LockyLuke.AuthAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LockyLuke.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : CustomBaseController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
       
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            var result = await _userService.CreateUserAsync(createUserDto);
            return ActionResultInstance(result);
        }

        [Authorize]
        [HttpGet("/api/[controller]/{userName}")]
        
        public async Task<IActionResult> GetUserByName(string userName)
        {
            return ActionResultInstance(await _userService.GetUserByUserNameAsync(userName));
        }

    }
}
