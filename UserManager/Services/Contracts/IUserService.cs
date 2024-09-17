using UserManager.DTOs;

namespace UserManager.Services.Contracts;

public interface IUserService
{
    Task Login(UserForLoginDto userDto);
    Task Logout();
    Task Block(int userId);
    Task Unblock(int userId);
    Task Delete(int userId);
    IEnumerable<UserDto> GetUsers();
    Task Register(UserForRegistrationDto userDto);
}