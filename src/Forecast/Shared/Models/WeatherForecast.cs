using System.Text.Json.Serialization;

public class WeatherForecast
{
    [JsonPropertyName("pulledOnUtc")]
    public DateTime PulledOnUtc { get; set; }
    [JsonPropertyName("features")]
    public List<WeatherFeature> Features { get; set; } = new List<WeatherFeature>();
}

public class WeatherFeature
{
    [JsonPropertyName("dateTime")]
    public DateTimeOffset DateTime { get; set; }
    [JsonPropertyName("l1TemperatureC")]
    public decimal L1TemperatureC { get; set; }
    [JsonPropertyName("l1Precipitation")]
    public decimal L1Precipitation { get; set; }
    [JsonPropertyName("l2TemperatureC")]
    public decimal L2TemperatureC { get; set; }
    [JsonPropertyName("l2Precipitation")]
    public decimal L2Precipitation { get; set; }
}