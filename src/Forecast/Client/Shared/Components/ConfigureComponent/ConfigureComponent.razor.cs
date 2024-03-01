using BlazorBootstrap;
using Forecast.Client.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace Forecast.Client.Shared.Components.ConfigureComponent;

public partial class ConfigureComponent : ComponentBase
{
    [Inject]
    IWeatherForecastService WeatherForecastService { get; set; } = default!;


    // Initialize the IsLoading properties
    [Parameter]
    public bool IsLoading { get; set; } = false;
    [Parameter]
    public EventCallback<bool> OnDataLoading { get; set; }

    // Initialize the Coordinates properties
    [Parameter]
    public Coordinates Coordinates { get; set; } = default!;
    [Parameter]
    public EventCallback<Coordinates> OnCoordinatesUpdated { get; set; }

    // Initialize the WeatherData properties
    [Parameter]
    public EventCallback<WeatherForecast> OnWeatherDataUpdated { get; set; }
    [Parameter]
    public WeatherForecast WeatherData { get; set; } = default!;

    Collapse CollapsableConfig = default!;
    public bool IsCollapsed { get; set; }


    protected override async Task OnInitializedAsync()
    {
        try
        {
            Console.WriteLine($"Loading toggle as: {IsCollapsed}");
            IsCollapsed = await LocalStorage.GetItemAsync<bool>("IsCollapsed");
            if (IsCollapsed)
            {
                await CollapsableConfig.HideAsync();
            }
            else
            {
                await CollapsableConfig.ShowAsync();
            }
        }
        catch
        {
            IsCollapsed = false;
            await CollapsableConfig.ShowAsync();
            Console.WriteLine($"Setting toggle to: {IsCollapsed}");
            await LocalStorage.SetItemAsync<bool>("IsCollapsed", IsCollapsed);
        }
    }

    private async Task ToggleContentAsync()
    {
        if (IsCollapsed)
        {
            IsCollapsed = false;
            await CollapsableConfig.ShowAsync();
        }
        else
        {
            IsCollapsed = true;
            await CollapsableConfig.HideAsync();
        }
        Console.WriteLine($"Setting toggle to: {IsCollapsed}");
        await LocalStorage.SetItemAsync<bool>("IsCollapsed", IsCollapsed);
    }

    private async Task HandleValidSubmit()
    {
        await OnDataLoading.InvokeAsync(true);
        try
        {
            if (Coordinates is null)
            {
                throw new ApplicationException("Coordinates are null");
            }

            // Update coordinates in local storage and notify parent
            await LocalStorage.SetItemAsync<Coordinates>("Coordinates", Coordinates);
            await OnCoordinatesUpdated.InvokeAsync(Coordinates);

            // Fetch weather data and notify parent
            var weatherData = await WeatherForecastService.GetWeatherForecast(Coordinates, 1);
            await LocalStorage.SetItemAsync<WeatherForecast>("WeatherForecast", weatherData);
            await OnWeatherDataUpdated.InvokeAsync(weatherData);
        }
        finally
        {
            await OnDataLoading.InvokeAsync(false);
        }
        StateHasChanged();
    }
}