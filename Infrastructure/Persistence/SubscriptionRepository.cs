namespace UsNotificationApi.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using UsNotificationApi.Domain.Subscriptions;
public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly SubscriptionDbContext _context;
    public SubscriptionRepository(SubscriptionDbContext context)
    {
        _context = context;
    }
    public async Task<Subscription> CreateSubscription(Subscription sub)
    {
        Subscription createdSub = _context.Subscriptions.Add(sub).Entity;
        await _context.SaveChangesAsync();
        return createdSub;
    }
    public async Task DeleteSubscription(Subscription sub)
    {
        _context.Remove(sub);
        await _context.SaveChangesAsync();
    }
    public async Task<List<Subscription>> GetSubscriptionByUserIds(List<string> userIds)
    {
        return await _context.Subscriptions.Where(sub => userIds.Contains(sub.UserId)).ToListAsync();
    }
    public async Task<Subscription?> GetSubscriptionsByEndPoint(string endpoint)
    {
        return await _context.Subscriptions.FirstOrDefaultAsync(sub => sub.Endpoint == endpoint);
    }
    public async Task<List<Subscription>> GetSubscriptionsByEndPoints(List<string> endpoints)
    {
        return await _context.Subscriptions.Where(sub => endpoints.Contains(sub.Endpoint)).ToListAsync();
    }
    public async Task<List<Subscription>> GetSubscriptionsByUserId(string userId)
    {
        return await _context.Subscriptions.Where(sub => sub.UserId == userId).ToListAsync();
    }
    public async Task<Subscription> UpdateSubscription(Subscription newSub)
    {
        _context.Subscriptions.Update(newSub);
        await _context.SaveChangesAsync();
        return newSub;
    }
}
