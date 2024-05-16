using Application.Interfaces.Mailing;
using Application.Interfaces.Services.Notifications;
using Application.Services.Notifications.Dtos;
using Application.Services.Notifications.Models;

using Domain.Entities.Identity;

namespace Application.Services.Notifications;

public class EmailNotificationService(IEmailSender emailSender) : INotificationService
{
    private const string EMAIL_DEFAULT_CULTURE = "fr";

    public async Task<SendNotificationResponseDto> SendForgotPasswordNotification(User user, string link)
    {
        ForgotPasswordNotificationModel model = new(
            user.Email ?? throw new InvalidOperationException("User email must be set."),
            EMAIL_DEFAULT_CULTURE,
            link);

        return await emailSender.SendAsync(model);
    }

    public async Task<SendNotificationResponseDto> SendTwoFactorAuthenticationCodeNotification(string destination,
        string code)
    {
        TwoFactorAuthenticationNotificationModel model = new(
            destination,
            EMAIL_DEFAULT_CULTURE,
            code);

        return await emailSender.SendAsync(model);
    }
}