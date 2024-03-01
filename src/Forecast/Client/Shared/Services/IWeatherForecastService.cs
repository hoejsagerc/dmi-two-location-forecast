namespace Forecast.Client.Shared.Services;
public interface IWeatherForecastService
{
    Task<WeatherForecast> GetWeatherForecast(Coordinates coordinates, int daysAhead);
}