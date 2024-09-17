using AutoMapper;
using UserManager.DTOs;
using UserManager.Entities;

namespace UserManager.MapperProfiles;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<UserForRegistrationDto, User>();
        CreateMap<UserForLoginDto, User>();
        CreateMap<User, UserDto>()
            .ConstructUsing(src => new UserDto(
                src.Id, 
                src.UserName, 
                src.Email, 
                src.RegistrationDate, 
                src.LastLoginTime, 
                src.LockoutEnd > DateTimeOffset.UtcNow ? "Blocked" : "Active"));
    }
}