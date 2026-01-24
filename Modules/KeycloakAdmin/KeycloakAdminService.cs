using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class KeycloakAdminService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    public KeycloakAdminService(HttpClient client, IConfiguration config)
    {
        _httpClient = client;
        _config = config;
    }

    private async Task<string> GetAccessTokenAsync()
    {
        // var tokenUrl = $"{_config["Keycloak:Authority"]}/realms/master/protocol/openid-connect/token";
        var tokenUrl = "https://auth.ade-dev.fr/realms/ustest/protocol/openid-connect/token";

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("client_id", "usapitest"),
            new KeyValuePair<string, string>("client_secret", "7EHZIDeOsoJb5u6I5rbTL1YwF1k26Zmy"),
        });

        // var response = await _httpClient.PostAsync(tokenUrl, content);
        var response = await _httpClient.PostAsync(tokenUrl, content);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Keycloak token error {response.StatusCode}: {error}");
        }
        response.EnsureSuccessStatusCode();


        var body = await response.Content.ReadAsStringAsync();
        using var json = JsonDocument.Parse(body);
        var accessToken = json.RootElement.GetProperty("access_token").GetString()!;
        Console.WriteLine(accessToken);
        return accessToken;
    }

    public async Task<List<KeycloakUser>> GetUsersAsync()
    {
        var token = await GetAccessTokenAsync();

        var realm = _config["Keycloak:Realm"];
        // var url = $"{_config["Keycloak:Authority"]}/admin/realms/{realm}/users";
        var url = "https://auth.ade-dev.fr/admin/realms/ustest/users";

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        Console.WriteLine(json);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<List<KeycloakUser>>(json, options)!;
    }


    public async Task<string> SetUserEnabled(string id, bool enabled)
    {
        var token = await GetAccessTokenAsync();
        var url = $"https://auth.ade-dev.fr/admin/realms/ustest/users/{id}";

        var payload = JsonSerializer.Serialize(new { enabled });
        var request = new HttpRequestMessage(HttpMethod.Put, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = new StringContent(payload, Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();

        return json;
    }

    public async Task<string> SetEmailVerified(string id, bool emailVerified)
    {
        var token = await GetAccessTokenAsync();
        var url = $"https://auth.ade-dev.fr/admin/realms/ustest/users/{id}";

        var payload = JsonSerializer.Serialize(new { emailVerified });
        var request = new HttpRequestMessage(HttpMethod.Put, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = new StringContent(payload, Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();

        return json;
    }
}