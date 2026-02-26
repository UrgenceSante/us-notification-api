using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using UsNotificationApi.Application.Users;
using UsNotificationApi.Domain.Users;

namespace UsNotificationApi.Infrastructure.Keycloak;

public class KeycloakAdminService : IUserAdminService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public KeycloakAdminService(HttpClient client, IConfiguration config)
    {
        _httpClient = client;
        _config = config;
    }

    private string UserUrl(string id) =>
        $"{_config["Keycloak:Authority"]}/admin/realms/{_config["Keycloak:Realm"]}/users/{id}";

    private async Task<string> GetAccessTokenAsync()
    {
        var tokenUrl = $"{_config["Keycloak:Authority"]}/realms/{_config["Keycloak:Realm"]}/protocol/openid-connect/token";

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("client_id", _config["Keycloak:ClientId"]!),
            new KeyValuePair<string, string>("client_secret", _config["Keycloak:ClientSecret"]!),
        });

        var response = await _httpClient.PostAsync(tokenUrl, content);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Keycloak token error {response.StatusCode}: {error}");
        }

        var body = await response.Content.ReadAsStringAsync();
        using var json = JsonDocument.Parse(body);
        return json.RootElement.GetProperty("access_token").GetString()!;
    }

    public async Task<List<KeycloakUser>> GetUsersAsync()
    {
        var token = await GetAccessTokenAsync();
        var url = $"{_config["Keycloak:Authority"]}/admin/realms/{_config["Keycloak:Realm"]}/users";

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<KeycloakUser>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<string> SetUserEnabled(string id, bool enabled)
    {
        var token = await GetAccessTokenAsync();
        var request = new HttpRequestMessage(HttpMethod.Put, UserUrl(id));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = new StringContent(JsonSerializer.Serialize(new { enabled }), Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> SetEmailVerified(string id, bool emailVerified)
    {
        var token = await GetAccessTokenAsync();
        var request = new HttpRequestMessage(HttpMethod.Put, UserUrl(id));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = new StringContent(JsonSerializer.Serialize(new { emailVerified }), Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        return await response.Content.ReadAsStringAsync();
    }
}