using Microsoft.AspNetCore.Mvc;
using UsNotificationApi.Application.Users;

namespace UsNotificationApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserAdminService _userAdminService;

    public UsersController(IUserAdminService userAdminService)
    {
        _userAdminService = userAdminService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var users = await _userAdminService.GetUsersAsync();
        return Ok(users);
    }

    [HttpPut("{id}/enabled")]
    public async Task<IActionResult> Put(string id, [FromBody] bool enabled)
    {
        await _userAdminService.SetUserEnabled(id, enabled);
        return Ok(enabled);
    }

    [HttpPut("{id}/emailVerified")]
    public async Task<IActionResult> UpdateEmailVerified(string id, [FromBody] bool emailVerified)
    {
        await _userAdminService.SetEmailVerified(id, emailVerified);
        return Ok(emailVerified);
    }
}
