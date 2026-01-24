using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly KeycloakAdminService _kc;

    public UsersController(KeycloakAdminService kc)
    {
        _kc = kc;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var users = await _kc.GetUsersAsync();
        return Ok(users);
    }

    [HttpPut("{id}/enabled")]
    public async Task<IActionResult> Put(string id, [FromBody] bool enabled)
    {
        Console.WriteLine(id);
        await _kc.SetUserEnabled(id, enabled);
        return Ok(enabled);
    }

    [HttpPut("{id}/emailVerified")]
    public async Task<IActionResult> UpdateEmailVerified(string id, [FromBody] bool emailVerified)
    {
        await _kc.SetEmailVerified(id, emailVerified);
        return Ok(emailVerified);
    }
}