using System.Text.Json;
using ApexCharts;
using Microsoft.AspNetCore.Components;

namespace Forecast.Client.Pages;

public partial class Index : ComponentBase
{
    public Coordinates Coordinates { get; set; } = new Coordinates();
    public WeatherForecast WeatherData { get; set; } = new WeatherForecast();
    public ApexChartOptions<WeatherFeature> options { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        Coordinates = await LocalStorage.GetItemAsync<Coordinates>("Coordinates") ?? new Coordinates();
        WeatherData = await LocalStorage.GetItemAsync<WeatherForecast>("WeatherForecast") ?? new WeatherForecast();

        options.Yaxis = new List<YAxis>();

        options.Yaxis.Add(new YAxis
        {
            Title = new AxisTitle { Text = "Temperature (°C)" },
            SeriesName = "temp_loc1",
            DecimalsInFloat = 1
        });

        options.Yaxis.Add(new YAxis
        {
            Show = false,
            Title = new AxisTitle { Text = "Temperature (°C)" },
            SeriesName = "temp_loc2",
            DecimalsInFloat = 1
        });

        options.Yaxis.Add(new YAxis
        {
            Title = new AxisTitle { Text = "Precipitation (mm)" },
            SeriesName = "prec_loc1",
            DecimalsInFloat = 1,
            Opposite = true
        });

        options.Yaxis.Add(new YAxis
        {
            Show = false,
            Title = new AxisTitle { Text = "Precipitation (mm)" },
            SeriesName = "prec_loc2",
            DecimalsInFloat = 1,
            Opposite = true
        });
    }

    private async Task HandleValidSubmit()
    {
        Console.WriteLine("Form submitted");
        if (Coordinates is null)
        {
            throw new ApplicationException("Coordinates are null");
        }
        await LocalStorage.SetItemAsync<Coordinates>("Coordinates", Coordinates);
        WeatherData = await GetWeatherForecast(Coordinates);
        await LocalStorage.SetItemAsync<WeatherForecast>("WeatherForecast", WeatherData);
        StateHasChanged();
    }



    private async Task<WeatherForecast> GetWeatherForecast(Coordinates coordinates)
    {
        var json = JsonSerializer.Serialize(coordinates);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await Http.PostAsync("WeatherForecast", content);

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
        Console.WriteLine(forecast.Features[0].L1Precipitation);
        Console.WriteLine(forecast.Features[1].L1Precipitation);
        Console.WriteLine(forecast.Features[2].L1Precipitation);
        Console.WriteLine(forecast.Features[3].L1Precipitation);
        return forecast;
    }
}