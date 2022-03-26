namespace DistributedCachingPoC.Infrastructure.WeatherForecast.QueryHandlers;

using DistributedCachingPoC.Infrastructure.Interfaces;
using DistributedCachingPoC.Application.WeatherForecast.ViewModels;
using DistributedCachingPoC.Application.WeatherForecast.Queries;

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
