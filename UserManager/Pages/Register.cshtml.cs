using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UserManager.DTOs;
using UserManager.Services.Contracts;

namespace UserManager.Pages;

public class Register(IUserService userService) : PageModel
{
    [BindProperty]
    public UserForRegistrationDto User { get; set; }
    
    public void OnGet()
    {
        
    }
    
    public async Task<IActionResult> OnPost()
    {
        try
        {
            await userService.Register(User);
            return RedirectToPage(nameof(Index));
        }
        catch (DbUpdateException e)
        {
            ModelState.AddModelError("", "Email already in use");
            return Page();
        }
        catch (Exception e)
        {
            ModelState.AddModelError("", e.Message);
            return Page();
        }
    }
}