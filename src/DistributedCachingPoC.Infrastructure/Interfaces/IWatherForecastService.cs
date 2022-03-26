namespace DistributedCachingPoC.Infrastructure.Interfaces;

using DistributedCachingPoC.Application.WeatherForecast.ViewModels;

public interface IWatherForecastService
{
    Task<IEnumerable<WeatherForecast>> GetWeatherForecastsAsync(int count = 5, CancellationToken cancellationToken = default);
}
