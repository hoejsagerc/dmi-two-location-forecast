using System.Text.Json;

namespace Forecast.Client.Shared.Services;

public class WeatherForecastService : IWeatherForecastService
{
    private readonly HttpClient _httpClient;

    public WeatherForecastService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WeatherForecast> GetWeatherForecast(Coordinates coordinates, int daysAhead)
    {
        var json = JsonSerializer.Serialize(coordinates);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"WeatherForecast/{daysAhead}", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException(responseContent);
        }

        var forecast = JsonSerializer.Deserialize<WeatherForecast>(responseContent);
        if (forecast is null)
        {
            throw new ApplicationException("Deserialization failed");
        }
        return forecast;
    }
}