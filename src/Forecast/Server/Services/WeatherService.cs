using System.Text;
using System.Text.Json;

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

    public async Task<WeatherForecast> GetForecastAsync(Coordinates coordinates)
    {
        ForecastResponse loc1Data = await GetForecastAsync(coordinates.PrimaryLatitude,
            coordinates.PrimaryLongitude);

        ForecastResponse loc2Data = await GetForecastAsync(coordinates.SecondaryLatitude,
            coordinates.SecondaryLongitude);

        List<WeatherFeature> features = new List<WeatherFeature>();

        for (int i = 0; i < loc1Data.Domain.Axes.T.Values.Count(); i++)
        {
            var feature = new WeatherFeature
            {
                DateTime = loc1Data.Domain.Axes.T.Values[i],
                L1Precipitation = Math.Round((decimal)loc1Data.Ranges.TotalPrecipitation.Values[i], 1),
                L1TemperatureC = Math.Round((decimal)(loc1Data.Ranges.Temperature0M.Values[i] - KelvinToCelsius), 1),
                L2Precipitation = Math.Round((decimal)loc2Data.Ranges.TotalPrecipitation.Values[i], 1),
                L2TemperatureC = Math.Round((decimal)(loc2Data.Ranges.Temperature0M.Values[i] - KelvinToCelsius), 1)
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

    private string BuildUrl(string latitude, string longitude)
    {
        var baseUrl = _configuration["WeatherService:BaseUrl"];
        var apiKey = _configuration["WeatherService:ApiKey"];
        var builder = new StringBuilder();

        builder.Append(baseUrl)
            .Append($"?coords=POINT({longitude} {latitude})")
            .Append("&parameter-name=temperature-0m")
            .Append("&parameter-name=total-precipitation")
            .Append("&crs=crs84")
            .Append($"&api-key={apiKey}");

        return builder.ToString();
    }

    public async Task<ForecastResponse> GetForecastAsync(string latitude, string longitude)
    {
        var url = BuildUrl(latitude, longitude);
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