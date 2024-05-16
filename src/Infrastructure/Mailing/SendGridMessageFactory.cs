using Application.Services.Notifications.Models;

using AutoMapper;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using SendGrid.Helpers.Mail;

namespace Infrastructure.Mailing;

public class SendGridMessageFactory(
    IHostEnvironment webHostEnvironment,
    IOptions<MailingSettings> mailingSettings,
    IMapper mapper)
    : ISendGridMessageFactory
{
    private readonly MailingSettings _mailingSettings = mailingSettings.Value;

    public SendGridMessage CreateFromModel<TModel>(TModel model) where TModel : NotificationModel
    {
        SendGridMessage msg = new()
        {
            From = new EmailAddress(_mailingSettings.FromAddress, _mailingSettings.FromName),
            TemplateId = model.TemplateId()
        };

        if (model.Attachments.Count != 0)
        {
            msg.Attachments = model.Attachments.Select(mapper.Map<Attachment>).ToList();
        }

        msg.SetTemplateData(model.TemplateData());

        if (webHostEnvironment.IsProduction())
        {
            msg.AddTo(model.Destination);
        }
        else if (webHostEnvironment.IsDevelopment())
        {
            msg.AddTo(_mailingSettings.ToAddressForDevelopment);
        }

        return msg;
    }
}