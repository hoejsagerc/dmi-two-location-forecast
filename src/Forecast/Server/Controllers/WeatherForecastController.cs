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

    [HttpPost]
    public async Task<WeatherForecast> Get([FromBody] Coordinates coordinates)
    {
        WeatherForecast forecast = await _weatherService.GetForecastAsync(coordinates);
        return forecast;
    }
}
