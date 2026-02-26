using Microsoft.AspNetCore.Mvc;
using UsNotificationApi.Application.Subscriptions;
using UsNotificationApi.Domain.Subscriptions;

namespace UsNotificationApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionController(ISubscriptionService service)
    {
        _subscriptionService = service;
    }

    [HttpPost]
    public async Task<ActionResult<Subscription>> CreateSubscription([FromBody] SubscriptionCreateDTO newSub)
    {
        Subscription myNewSub = await _subscriptionService.CreateSubscription(newSub);
        return Ok(myNewSub);
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<List<Subscription>>> GetSubscriptionsByUser(string userId)
    {
        var subs = await _subscriptionService.GetSubscriptionByUser(userId);
        return Ok(subs);
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteSubscription([FromBody] DeleteSubscriptionDTO dto)
    {
        var existing = await _subscriptionService.GetSubscriptionByEndpoint(dto.Endpoint);
        if (existing is null) return NotFound();

        await _subscriptionService.DeleteSubscription(dto.Endpoint);
        return NoContent();
    }
}
