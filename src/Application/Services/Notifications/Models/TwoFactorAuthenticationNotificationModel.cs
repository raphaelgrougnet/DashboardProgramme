namespace Application.Services.Notifications.Models;

public class TwoFactorAuthenticationNotificationModel(string destination, string locale, string code)
    : NotificationModel(destination, locale)
{
    public string Code { get; set; } = code;

    public override object TemplateData()
    {
        return new { Code };
    }

    public override string TemplateId()
    {
        return Locale == "fr" ? "d-b6b894b660614a289e1d3e0e1cc81c17" : "d-0ba3cba8d527436dbebe4ee440104d77";
    }
}