namespace DistributedCachingPoC.Infrastructure.WeatherForecast.QueryHandlers;

using System.Threading;
using System.Text.Json;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using DistributedCachingPoC.Infrastructure.Interfaces;
using DistributedCachingPoC.Application.WeatherForecast.Models;
using DistributedCachingPoC.Application.WeatherForecast.Queries;
using MediatR;

internal sealed class GetWatherForecastHandler : IRequestHandler<GetWatherForecast, IEnumerable<WeatherForecast>>
{
    private readonly ILogger<GetWatherForecastHandler> logger;
    private readonly IDistributedCache cache;
    private readonly DistributedCachingOptions options;
    private readonly IWatherForecastService service;

    public GetWatherForecastHandler(ILogger<GetWatherForecastHandler> logger, IDistributedCache cache, IOptions<DistributedCachingOptions> options, IWatherForecastService service) =>
    (this.logger, this.cache, this.options, this.service) = (logger, cache, options.Value, service);

    public async Task<IEnumerable<WeatherForecast>> Handle(GetWatherForecast request, CancellationToken cancellationToken)
    {
        var cached = await cache.GetStringAsync(options.CacheKey);
        if (cached == null)
        {
            var result = await this.service.GetWeatherForecastsAsync();

            await cache.SetStringAsync(
            options.CacheKey,
            System.Text.Json.JsonSerializer.Serialize(result),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                    SlidingExpiration = TimeSpan.FromSeconds(60)
                });

            return result;
        }

        return await Task.FromResult(JsonSerializer.Deserialize<IEnumerable<WeatherForecast>>(cached));

    }
}
