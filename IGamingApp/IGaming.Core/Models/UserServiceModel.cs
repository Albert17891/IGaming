namespace IGaming.Core.Models;

public class UserServiceModel
{
    public int Id { get; set; } 
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}