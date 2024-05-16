using System.Text.Json;

using Application.Interfaces.Mailing;
using Application.Services.Notifications.Dtos;
using Application.Services.Notifications.Models;

using Microsoft.Extensions.Logging;

using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Mailing;

public class SendGridSender(
    ILogger<SendGridSender> logger,
    ISendGridClient sendGridClient,
    ISendGridMessageFactory sendGridMessageFactory)
    : IEmailSender
{
    public async Task<SendNotificationResponseDto> SendAsync<TModel>(TModel model) where TModel : NotificationModel
    {
        SendGridMessage msg = sendGridMessageFactory.CreateFromModel(model);
        Response? response = await sendGridClient.SendEmailAsync(msg);

        if (response.IsSuccessStatusCode)
        {
            return new SendNotificationResponseDto(response.IsSuccessStatusCode);
        }

        List<string> errors = await GetErrorListFromResponse(response);
        logger.LogError("Error occured while sending email. Errors : {errors}", JsonSerializer.Serialize(errors));

        return new SendNotificationResponseDto(response.IsSuccessStatusCode, errors);
    }

    private async Task<List<string>> GetErrorListFromResponse(Response response)
    {
        Dictionary<string, dynamic>? deserializedResponse = await response.DeserializeResponseBodyAsync();
        string stringErrorList = Convert.ToString(deserializedResponse.First().Value);
        List<SendGridError>? errors = JsonSerializer.Deserialize<List<SendGridError>>(stringErrorList);
        return errors == null ? [] : errors.Select(x => x.Message).ToList();
    }
}