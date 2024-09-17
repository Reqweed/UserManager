using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using UserManager.DTOs;
using UserManager.Entities;
using UserManager.Services.Contracts;

namespace UserManager.Services.Implementations;

public class UserService(SignInManager<User> signInManager, IMapper mapper) : IUserService
{
    public async Task Login(UserForLoginDto userDto)
    {
        if (userDto is null)
            throw new Exception("Try again later");

        var user = await signInManager.UserManager.FindByEmailAsync(userDto.Email);
        if(user is null)
            throw new Exception("Incorrect password or email");
        
        var result = await signInManager.CheckPasswordSignInAsync(user, userDto.Password, false);
        if (result.IsLockedOut)
            throw new Exception("User is locked out.");
        if (!result.Succeeded)
            throw new Exception("Incorrect password or email");
        
        await signInManager.SignInAsync(user, isPersistent: false);
        
        user.LastLoginTime = DateTime.UtcNow;
        await signInManager.UserManager.UpdateAsync(user);
        await CreateClaims(user.UserName);
    }

    public async Task Logout()
    {
        await signInManager.SignOutAsync();
    }

    public async Task Block(int userId)
    {
        var user = await signInManager.UserManager.FindByIdAsync(userId.ToString())
                   ?? throw new Exception("Something went wrong");
                   
        user.LockoutEnd = DateTimeOffset.MaxValue;
        await signInManager.UserManager.UpdateAsync(user);
    }

    public async Task Unblock(int userId)
    {
        var user = await signInManager.UserManager.FindByIdAsync(userId.ToString())
                   ?? throw new Exception("Something went wrong");

        user.LockoutEnd = null;
        await signInManager.UserManager.UpdateAsync(user);
    }

    public async Task Delete(int userId)
    {
        var user = await signInManager.UserManager.FindByIdAsync(userId.ToString())
                   ?? throw new Exception("Something went wrong");
        
        await signInManager.UserManager.DeleteAsync(user);
    }

    public IEnumerable<UserDto> GetUsers()
    {
        var users = signInManager.UserManager.Users;

        var usersDto = mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(users);

        return usersDto;
    }

    public async Task Register(UserForRegistrationDto userDto)
    {
        if (userDto is null) 
            throw new Exception("Try again later");
            
        var user = mapper.Map<UserForRegistrationDto, User>(userDto);
        
        user.RegistrationDate = DateTime.UtcNow;
        var result = await signInManager.UserManager.CreateAsync(user, userDto.Password);
        if (!result.Succeeded)
            throw new Exception(result.Errors.FirstOrDefault()?.Description);
        
        await signInManager.SignInAsync(user, isPersistent: false);
        await CreateClaims(user.UserName);
    }

    private async Task CreateClaims(string userName)
    {
        var claims = new List<Claim> { new (ClaimTypes.Name, userName) };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(claimsIdentity);
        
        await signInManager.Context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }
}