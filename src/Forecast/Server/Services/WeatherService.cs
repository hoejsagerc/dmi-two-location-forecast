using System.Text;
using System.Text.Json;
using Forecast.Shared.Models;

public class WeatherService : IWeatherService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public WeatherService(IConfiguration configuration)
    {
        _configuration = configuration;
        _httpClient = new HttpClient();
    }

    const float KelvinToCelsius = 273.15f;

    public async Task<WeatherForecast> GetForecastAsync(Coordinates coordinates, int daysAhead)
    {
        ForecastResponse loc1Data = await GetForecastAsync(coordinates.PrimaryLatitude,
            coordinates.PrimaryLongitude, daysAhead);

        ForecastResponse loc2Data = await GetForecastAsync(coordinates.SecondaryLatitude,
            coordinates.SecondaryLongitude, daysAhead);

        List<WeatherFeature> features = new List<WeatherFeature>();

        for (int i = 0; i < loc1Data.Domain.Axes.T.Values.Count(); i++)
        {
            var feature = new WeatherFeature
            {
                DateTime = loc1Data.Domain.Axes.T.Values[i],
                L1Precipitation = Math.Round((decimal)loc1Data.Ranges.TotalPrecipitation.Values[i], 1),
                L1TemperatureC = Math.Round((decimal)(loc1Data.Ranges.Temperature0M.Values[i] - KelvinToCelsius), 1),
                L1GustWindSpeed = Math.Round((decimal)loc1Data.Ranges.GustWindSpeed10M.Values[i], 1),
                L1WindSpeed = Math.Round((decimal)loc1Data.Ranges.WindSpeed10M.Values[i], 1),
                L2Precipitation = Math.Round((decimal)loc2Data.Ranges.TotalPrecipitation.Values[i], 1),
                L2TemperatureC = Math.Round((decimal)(loc2Data.Ranges.Temperature0M.Values[i] - KelvinToCelsius), 1),
                L2GustWindSpeed = Math.Round((decimal)loc2Data.Ranges.GustWindSpeed10M.Values[i], 1),
                L2WindSpeed = Math.Round((decimal)loc2Data.Ranges.WindSpeed10M.Values[i], 1),
            };

            features.Add(feature);
        }

        var weatherforecast = new WeatherForecast
        {
            PulledOnUtc = DateTime.UtcNow,
            Features = features
        };

        return weatherforecast;
    }

    private string BuildUrl(string latitude, string longitude, int daysAhead)
    {
        var baseUrl = _configuration["WeatherService:BaseUrl"];
        var apiKey = _configuration["WeatherService:ApiKey"];
        var builder = new StringBuilder();
        DateTime twoDaysLaterAt23 = DateTime.UtcNow.AddDays(daysAhead).Date.AddHours(23);

        builder.Append(baseUrl)
            .Append($"?coords=POINT({longitude} {latitude})")
            .Append("&parameter-name=temperature-0m")
            .Append("&parameter-name=total-precipitation")
            .Append("&parameter-name=gust-wind-speed-10m")
            .Append("&parameter-name=wind-speed-10m")
            .Append("&crs=crs84")
            .Append($"&datetime=../{twoDaysLaterAt23.ToString("yyyy-MM-dd'T'HH:mm:ss")}Z")
            .Append($"&api-key={apiKey}");

        return builder.ToString();
    }

    public async Task<ForecastResponse> GetForecastAsync(string latitude, string longitude, int daysAhead)
    {
        var url = BuildUrl(latitude, longitude, daysAhead);
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to fetch data: {response.ReasonPhrase}");
        }
        var content = await response.Content.ReadAsStringAsync();
        var forecastResponse = JsonSerializer.Deserialize<ForecastResponse>(content);
        if (forecastResponse is null)
        {
            throw new InvalidOperationException("Failed to deserialize response");
        }
        return forecastResponse;
    }
}