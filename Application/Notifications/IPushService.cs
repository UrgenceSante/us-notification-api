namespace UsNotificationApi.Application.Notifications;

public interface IPushService
{
    Task<NotificationResult> NotifyAllUserIds(NotificationCmd cmd, string payload);
}
