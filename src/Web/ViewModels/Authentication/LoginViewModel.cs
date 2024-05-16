using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Authentication;

public class LoginViewModel
{
    [Required] public string UserName { get; set; } = default!;

    [Required] public string Password { get; set; } = default!;

    public string? ReturnUrl { get; set; }
}