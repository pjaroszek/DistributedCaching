namespace DistributedCachingPoC.Application.WeatherForecast.Models;

public sealed class WeatherForecast
{
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF { get; set; }

    public string Summary { get; set; } = string.Empty;
}
