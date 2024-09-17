namespace UserManager.DTOs;

public record UserDto(
    int UserId, 
    string UserName, 
    string Email, 
    DateTime RegistrationDate, 
    DateTime? LastLoginTime, 
    string Status);