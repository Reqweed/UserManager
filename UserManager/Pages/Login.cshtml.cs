using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManager.DTOs;
using UserManager.Services.Contracts;

namespace UserManager.Pages;

public class Login(IUserService userService) : PageModel
{
    [BindProperty]
    public UserForLoginDto User { get; set; }
    
    public void OnGet()
    {
        
    }

    public async Task<IActionResult> OnPost()
    {
        try
        {
            await userService.Login(User);
            return RedirectToPage(nameof(Index));
        }
        catch (Exception e)
        {
            ModelState.AddModelError("",e.Message);
            return Page();
        }
    }
}