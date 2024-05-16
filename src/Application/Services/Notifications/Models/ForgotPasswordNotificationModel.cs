namespace Application.Services.Notifications.Models;

public class ForgotPasswordNotificationModel(string destination, string locale, string link)
    : NotificationModel(destination, locale)
{
    public string Link { get; set; } = link;

    public override object TemplateData()
    {
        return new { ButtonUrl = Link };
    }

    public override string TemplateId()
    {
        return Locale == "fr" ? "d-ccea0bf1594048259d41fb52c2c23614" : "d-6bceb5f892064a7b95cc03fe16b45943";
    }
}