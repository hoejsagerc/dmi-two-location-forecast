using System.Text.Json;
using ApexCharts;
using Forecast.Client.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Forecast.Client.Pages;

public partial class Index : ComponentBase
{
    [Inject]
    IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    IWeatherForecastService WeatherForecastService { get; set; } = default!;

    private ApexChart<WeatherFeature> precChart = default!;
    private ApexChart<WeatherFeature> windChart = default!;
    private ApexChart<WeatherFeature> tempChart = default!;

    public bool IsLoading { get; set; } = false;
    public string CallOutPrecText { get; set; } = string.Empty;
    public string CallOutWindText { get; set; } = string.Empty;
    public Coordinates Coordinates { get; set; } = new Coordinates();
    public WeatherForecast WeatherData { get; set; } = new WeatherForecast();

    protected override async Task OnInitializedAsync()
    {
        Coordinates = await LocalStorage.GetItemAsync<Coordinates>("Coordinates") ?? new Coordinates();
        WeatherData = await LocalStorage.GetItemAsync<WeatherForecast>("WeatherForecast") ?? new WeatherForecast();
        if (!string.IsNullOrEmpty(Coordinates.PrimaryLatitude)
            && !string.IsNullOrEmpty(Coordinates.PrimaryLongitude)
            && !string.IsNullOrEmpty(Coordinates.SecondaryLatitude)
            && !string.IsNullOrEmpty(Coordinates.SecondaryLongitude))
        {
            await LoadData();
        }
    }


    private async Task LoadData()
    {
        if (WeatherData.Features.Count() == 0 || WeatherData.PulledOnUtc.Date != DateTime.UtcNow.Date)
        {
            WeatherData = await WeatherForecastService.GetWeatherForecast(Coordinates, 1);
            await LocalStorage.SetItemAsync<WeatherForecast>("WeatherForecast", WeatherData);
            await windChart.RenderAsync();
            await tempChart.RenderAsync();
            await precChart.RenderAsync();
            StateHasChanged();
        }
        CheckForMessage();
    }

    private void CheckForMessage()
    {
        List<WeatherFeature> loc1Morning = WeatherData.Features
            .Where(f => f.DateTime.Hour >= 5 && f.DateTime.Hour <= 9).ToList();
        List<WeatherFeature> loc2Afternoon = WeatherData.Features
            .Where(f => f.DateTime.Hour >= 13 && f.DateTime.Hour <= 17).ToList();

        var avgLoc1Precipitation = Math.Round(loc1Morning.Select(f => f.L1Precipitation).Average(), 2);
        var avgLoc2Precipitation = Math.Round(loc1Morning.Select(f => f.L2Precipitation).Average(), 2);

        if (avgLoc1Precipitation > (decimal)0.5 || avgLoc2Precipitation > (decimal)0.5)
        {
            CallOutPrecText = $"The average precipitation on location 1 between 05:00 - 09:00 is {avgLoc1Precipitation}mm and on location 2 between 13:00 - 17:00 is {avgLoc2Precipitation}mm.";
        }

        var avgLoc1WindSpeed = Math.Round(loc1Morning.Select(f => f.L1WindSpeed).Average(), 2);
        var avgLoc2WindSpeed = Math.Round(loc1Morning.Select(f => f.L2WindSpeed).Average(), 2);

        if (avgLoc1WindSpeed > (decimal)8.0 || avgLoc2WindSpeed > (decimal)8.0)
        {
            CallOutWindText = $"The average windspeed on location 1 between 05:00 - 09:00 is {avgLoc1Precipitation}m/s and on location 2 between 13:00 - 17:00 is {avgLoc2Precipitation}m/s.";
        }
    }


    // ------- EVENT CALLBACKS -------
    private async Task OnWeatherDataUpdated(WeatherForecast data)
    {
        WeatherData = data;
        Console.WriteLine("Refreshing charts");
        await precChart.RenderAsync();
        await tempChart.RenderAsync();
        CheckForMessage();
        await InvokeAsync(() =>
        {
            StateHasChanged();
        });
    }

    private async Task OnDataLoading(bool isLoading)
    {
        IsLoading = isLoading;
        Console.WriteLine($"IsLoading: {IsLoading}");
        await InvokeAsync(() =>
        {
            StateHasChanged();
        });
    }

    private async Task OnCoordinatesUpdated(Coordinates coords)
    {
        Coordinates = coords;
        await InvokeAsync(() =>
        {
            StateHasChanged();
        });
    }
}