using System.Text.Json.Serialization;

namespace Tixxp.WebApp.Models.Authorization;

public class LoginRequestModel
{
    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }
}
