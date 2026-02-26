using UsNotificationApi.Domain.Subscriptions;

namespace UsNotificationApi.Application.Subscriptions;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _subscriptionRepository;

    public SubscriptionService(ISubscriptionRepository repository)
    {
        _subscriptionRepository = repository;
    }

    public async Task<Subscription> CreateSubscription(SubscriptionCreateDTO sub)
    {

        Subscription? existing = await _subscriptionRepository.GetSubscriptionsByEndPoint(sub.Endpoint);
        if (existing is not null)
        {
            existing.Auth = sub.Auth;
            existing.P256dh = sub.P256dh;
            existing.LastUsed = DateTime.UtcNow;
            return await _subscriptionRepository.UpdateSubscription(existing);
        }

        Subscription newSub = new()
        {
            UserId = sub.UserId,
            Auth = sub.Auth,
            P256dh = sub.P256dh,
            Endpoint = sub.Endpoint,
            NavigatorName = sub.NavigatorName,
            OsName = sub.OsName,
            OsVersion = sub.OsVersion,
            CreatedDate = DateTime.UtcNow,
            LastUsed = DateTime.UtcNow,
        };
        return await _subscriptionRepository.CreateSubscription(newSub);
    }

    public async Task<List<Subscription>> GetSubscriptionByUser(string userId)
    {
        return await _subscriptionRepository.GetSubscriptionsByUserId(userId);
    }

    public async Task<Subscription?> GetSubscriptionByEndpoint(string endpoint)
    {
        return await _subscriptionRepository.GetSubscriptionsByEndPoint(endpoint);
    }

    public async Task DeleteSubscription(string endpoint)
    {
        Subscription? sub = await _subscriptionRepository.GetSubscriptionsByEndPoint(endpoint);
        if (sub is not null)
            await _subscriptionRepository.DeleteSubscription(sub);
    }
}