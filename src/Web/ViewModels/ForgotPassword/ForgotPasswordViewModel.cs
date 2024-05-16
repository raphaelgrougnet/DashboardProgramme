using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.ForgotPassword;

public class ForgotPasswordViewModel
{
    [Required] public string UserName { get; set; } = default!;
}