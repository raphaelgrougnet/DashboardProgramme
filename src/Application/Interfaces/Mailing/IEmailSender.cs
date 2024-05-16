using Application.Services.Notifications.Dtos;
using Application.Services.Notifications.Models;

namespace Application.Interfaces.Mailing;

public interface IEmailSender
{
    Task<SendNotificationResponseDto> SendAsync<TModel>(TModel model) where TModel : NotificationModel;
}