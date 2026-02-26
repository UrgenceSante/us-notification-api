using UsNotificationApi.Domain.Subscriptions;

namespace UsNotificationApi.Application.Subscriptions;

public interface ISubscriptionService
{
    Task<Subscription> CreateSubscription(SubscriptionCreateDTO sub);
    Task<List<Subscription>> GetSubscriptionByUser(string userId);
    Task<Subscription?> GetSubscriptionByEndpoint(string endpoint);
    Task DeleteSubscription(string endpoint);
}
