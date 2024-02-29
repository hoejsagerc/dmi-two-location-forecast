using System.Text.Json.Serialization;

public class Coordinates
{
    [JsonPropertyName("primaryLatitude")]
    public string PrimaryLatitude { get; set; } = string.Empty;
    [JsonPropertyName("primaryLongitude")]
    public string PrimaryLongitude { get; set; } = string.Empty;

    [JsonPropertyName("secondaryLatitude")]
    public string SecondaryLatitude { get; set; } = string.Empty;
    [JsonPropertyName("secondaryLongitude")]
    public string SecondaryLongitude { get; set; } = string.Empty;
}