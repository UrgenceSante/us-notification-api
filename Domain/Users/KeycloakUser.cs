namespace UsNotificationApi.Domain.Users;
public class KeycloakUser
{
    public string Id { get; set; } = "";
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public bool Enabled { get; set; }

    public bool EmailVerified { get; set; }

    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
}