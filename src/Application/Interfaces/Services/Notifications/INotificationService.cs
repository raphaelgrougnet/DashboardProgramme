using Application.Services.Notifications.Dtos;

using Domain.Entities.Identity;

namespace Application.Interfaces.Services.Notifications;

public interface INotificationService
{
    Task<SendNotificationResponseDto> SendForgotPasswordNotification(User user, string link);
    Task<SendNotificationResponseDto> SendTwoFactorAuthenticationCodeNotification(string destination, string code);
}