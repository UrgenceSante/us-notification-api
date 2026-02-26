using UsNotificationApi.Domain.Users;

namespace UsNotificationApi.Application.Users;

public interface IUserAdminService
{
    Task<List<KeycloakUser>> GetUsersAsync();
    Task<string> SetUserEnabled(string id, bool enabled);
    Task<string> SetEmailVerified(string id, bool emailVerified);
}
