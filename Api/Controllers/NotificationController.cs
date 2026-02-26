using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UsNotificationApi.Application.Notifications;

namespace UsNotificationApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly IPushService _pushService;

    public NotificationsController(IPushService pushService)
    {
        _pushService = pushService;
    }

    [HttpPost("notifyByIds")]
    public async Task<ActionResult<NotificationResult>> NotifyAllUserIds([FromBody] NotificationCmd cmd)
    {
        var payload = JsonConvert.SerializeObject(new
        {
            title = cmd.Title,
            message = cmd.Message
        });

        NotificationResult result = await _pushService.NotifyAllUserIds(cmd, payload);
        return Ok(result);
    }
}
