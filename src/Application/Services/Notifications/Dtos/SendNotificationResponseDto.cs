namespace Application.Services.Notifications.Dtos;

public class SendNotificationResponseDto(bool successful, IList<string>? errorMessages = null)
{
    public bool Successful { get; } = successful;
    public IEnumerable<string> ErrorMessages { get; } = errorMessages ?? new List<string>();
}