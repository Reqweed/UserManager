using Microsoft.AspNetCore.Identity;

namespace UserManager.Entities;

public class User : IdentityUser<int>
{
    public DateTime RegistrationDate { get; set; }
    public DateTime? LastLoginTime { get; set; } 
}