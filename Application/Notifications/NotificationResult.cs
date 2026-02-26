namespace UsNotificationApi.Application.Notifications;

public class NotificationResult
{
    public List<string> Notified { get; set; } = [];
    public List<string> Expired { get; set; } = [];
    public List<string> Failed { get; set; } = [];
}
