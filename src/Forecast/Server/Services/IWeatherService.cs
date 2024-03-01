public interface IWeatherService
{
    Task<WeatherForecast> GetForecastAsync(Coordinates coordinates, int daysAhead);
}