using System.Net;
using UsNotificationApi.Domain.Subscriptions;
using WebPush;

namespace UsNotificationApi.Application.Notifications;

public class PushService : IPushService
{
    private readonly VapidDetails _vapidDetails;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly ILogger<PushService> _logger;
    private readonly WebPushClient _pushClient = new();

    public PushService(IConfiguration configuration, ISubscriptionRepository pushSubRepository, ILogger<PushService> logger)
    {
        _vapidDetails = new VapidDetails(
            "mailto:alex.delesse01@gmail.com",
            configuration["VapidKeys:PublicKey"],
            configuration["VapidKeys:PrivateKey"]
        );
        _subscriptionRepository = pushSubRepository;
        _logger = logger;
    }

    public async Task<NotificationResult> NotifyAllUserIds(NotificationCmd cmd, string payload)
    {
        List<Subscription> byUser = await _subscriptionRepository.GetSubscriptionByUserIds(cmd.UserIds);
        List<Subscription> byEndpoint = await _subscriptionRepository.GetSubscriptionsByEndPoints(cmd.Endpoints);

        var subscriptions = byUser.Concat(byEndpoint).DistinctBy(sub => sub.Endpoint).ToList();

        var result = new NotificationResult();

        foreach (var sub in subscriptions)
        {
            var pushSub = new PushSubscription(endpoint: sub.Endpoint, auth: sub.Auth, p256dh: sub.P256dh);
            try
            {
                await _pushClient.SendNotificationAsync(subscription: pushSub, payload, vapidDetails: _vapidDetails);
                sub.LastUsed = DateTime.UtcNow;
                await _subscriptionRepository.UpdateSubscription(sub);
                result.Notified.Add(sub.Endpoint);
            }
            catch (WebPushException ex) when (ex.StatusCode == HttpStatusCode.Gone)
            {
                _logger.LogInformation("Subscription expired (410 Gone), deleting endpoint: {Endpoint}", sub.Endpoint);
                await _subscriptionRepository.DeleteSubscription(sub);
                result.Expired.Add(sub.Endpoint);
            }
            catch (WebPushException ex)
            {
                _logger.LogError(ex, "WebPush error for endpoint {Endpoint}: {StatusCode}", sub.Endpoint, ex.StatusCode);
                result.Failed.Add(sub.Endpoint);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error sending push notification to endpoint {Endpoint}", sub.Endpoint);
                result.Failed.Add(sub.Endpoint);
            }
        }

        return result;
    }
}
