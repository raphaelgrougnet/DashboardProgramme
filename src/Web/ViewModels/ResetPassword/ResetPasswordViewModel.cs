using Microsoft.Build.Framework;

namespace Web.ViewModels.ResetPassword;

public class ResetPasswordViewModel
{
    [Required] public string Password { get; set; } = default!;

    public string? RedirectUrl { get; set; } = string.Empty;
}