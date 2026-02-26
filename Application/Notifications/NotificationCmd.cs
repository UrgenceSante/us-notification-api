namespace UsNotificationApi.Application.Notifications;


public class NotificationCmd
{
    public List<string> UserIds { get; set; } = [];
    public List<string> Endpoints { get; set; } = [];
    public required string Title { get; set; }
    public required string Message { get; set; }

}