using Microsoft.Build.Framework;

namespace Web.ViewModels.Authentication;

public class TwoFactorViewModel
{
    [Required] public string TwoFactorCode { get; set; } = default!;

    [Required] public string UserName { get; set; } = default!;

    public string? ReturnUrl { get; set; }
}