using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManager.DTOs;
using UserManager.Services.Contracts;

namespace UserManager.Pages;

public class IndexModel(IUserService userService) : PageModel
{
    [BindProperty] public List<int> SelectedUsers { get; set; } = new();
    public IEnumerable<UserDto> Users { get; set; } = userService.GetUsers();
    
    public void OnGet()
    {
    }

    [Authorize]
    public async Task<IActionResult> OnPostLogout()
    {
        await userService.Logout();
        return RedirectToPage();
    }

    [Authorize]
    public async Task<IActionResult> OnPostBlockAsync()
    {
        foreach (var id in SelectedUsers)
        {
            await userService.Block(id);
        }

        return RedirectToPage();
    }

    [Authorize]
    public async Task<IActionResult> OnPostUnblockAsync()
    {
        foreach (var id in SelectedUsers)
        {
            await userService.Unblock(id);
        }

        return RedirectToPage();
    }
    
    [Authorize]
    public async Task<IActionResult> OnPostDeleteAsync()
    {
        foreach (var id in SelectedUsers)
        {
            await userService.Delete(id);
        }

        return RedirectToPage();
    }
}