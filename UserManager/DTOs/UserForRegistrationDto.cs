using System.ComponentModel.DataAnnotations;

namespace UserManager.DTOs;

public record UserForRegistrationDto(string UserName, [EmailAddress] string Email, string Password);