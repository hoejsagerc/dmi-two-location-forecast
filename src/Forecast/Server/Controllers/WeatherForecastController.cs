using Microsoft.AspNetCore.Mvc;

namespace Forecast.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    public WeatherForecastController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpPost("{daysAhead}")]
    public async Task<WeatherForecast> Get([FromBody] Coordinates coordinates, int daysAhead)
    {
        WeatherForecast forecast = await _weatherService.GetForecastAsync(coordinates, daysAhead);
        return forecast;
    }
}
