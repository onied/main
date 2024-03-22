using System.Text.Json.Serialization;

namespace Users.Dtos.VkUserInfoResponseDtos;

public class UserInfoResponse
{
    [JsonPropertyName("photo_200")]
    public string? Photo200 { get; set; }

    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }

    [JsonPropertyName("sex")]
    public int? Sex { get; set; }
}
