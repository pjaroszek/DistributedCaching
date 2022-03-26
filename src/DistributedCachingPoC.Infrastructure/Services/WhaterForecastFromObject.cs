namespace DistributedCachingPoC.Infrastructure.Services;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DistributedCachingPoC.Application.WeatherForecast.Models;
using DistributedCachingPoC.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

internal sealed class WhaterForecastFromObject : IWatherForecastService
{
    private readonly ILogger<WhaterForecastFromObject> logger;

    public WhaterForecastFromObject(ILogger<WhaterForecastFromObject> logger) => this.logger = logger;

    private static readonly string[] Summaries = new[]
    {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public async Task<IEnumerable<WeatherForecast>> GetWeatherForecastsAsync(int count = 5, CancellationToken cancellationToken = default)
    {
        this.logger.LogInformation("Preparation of weather data");

        var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        }).ToArray();

        return await Task.FromResult(result);
    }
}
