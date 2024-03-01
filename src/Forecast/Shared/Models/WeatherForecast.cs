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

    /// <summary>
    /// Location 1 Temperature in Celsius
    /// </summary>
    [JsonPropertyName("l1TemperatureC")]
    public decimal L1TemperatureC { get; set; }

    /// <summary>
    /// Location 1 Precipitation in mm
    /// </summary>
    [JsonPropertyName("l1Precipitation")]
    public decimal L1Precipitation { get; set; }

    /// <summary>
    /// Location 1 Gust Wind Speed in m/s at 10m above ground
    /// </summary>
    [JsonPropertyName("l1GustWindSpeed")]
    public decimal L1GustWindSpeed { get; set; }

    /// <summary>
    /// Location 1 Wind Speed in m/s at 10m above ground
    /// </summary>
    [JsonPropertyName("l1WindSpeed")]
    public decimal L1WindSpeed { get; set; }


    /// <summary>
    /// Location 2 Temperature in Celsius
    /// </summary>
    [JsonPropertyName("l2TemperatureC")]
    public decimal L2TemperatureC { get; set; }
    [JsonPropertyName("l2Precipitation")]

    /// <summary>
    /// Location 2 Temperature in Celsius
    /// </summary>
    public decimal L2Precipitation { get; set; }
    [JsonPropertyName("l2GustWindSpeed")]

    /// <summary>
    /// Location 2 Gust Wind Speed in m/s at 10m above ground
    /// </summary>
    public decimal L2GustWindSpeed { get; set; }

    /// <summary>
    /// Location 2 Wind Speed in m/s at 10m above ground
    /// </summary>
    [JsonPropertyName("l2WindSpeed")]
    public decimal L2WindSpeed { get; set; }

}