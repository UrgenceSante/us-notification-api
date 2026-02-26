using Microsoft.EntityFrameworkCore;
using UsNotificationApi.Domain.Subscriptions;

namespace UsNotificationApi.Infrastructure.Persistence;

public class SubscriptionDbContext : DbContext
{
    public SubscriptionDbContext(DbContextOptions<SubscriptionDbContext> options) : base(options)
    {

    }

    public DbSet<Subscription> Subscriptions { get; set; } = null!;
}