using System.ComponentModel.DataAnnotations;

namespace IGaming.Models.Request;

public class RegisterRequest
{
    [Required]
    public string UserName { get; set; }
    [Required]
    [EmailAddress(ErrorMessage = "value is not valid email address")]
    public string Email { get; set; }
    [Required]
    [MinLength(6, ErrorMessage = "Minimal Password length must be 6")]
    public string Password { get; set; }
    [Required]
    [MinLength(6, ErrorMessage = "Minimal Password length must be 6")]
    public string ConfirmPassword { get; set; }
}