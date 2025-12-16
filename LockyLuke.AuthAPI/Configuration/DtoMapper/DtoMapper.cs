using AutoMapper;
using LockyLuke.AuthAPI.Entities.Users;
using SharedLibrary.Dtos.User;

namespace LockyLuke.AuthAPI.Configuration.DtoMapper
{
    public class DtoMapper:Profile
    {
        public DtoMapper()
        {
            CreateMap<AppUserDto, AppUser>().ReverseMap();
        }
    }
}
