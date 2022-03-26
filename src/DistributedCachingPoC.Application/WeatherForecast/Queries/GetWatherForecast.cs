namespace DistributedCachingPoC.Application.WeatherForecast.Queries;

using ViewModels;

public sealed record GetWatherForecast : IRequest<IEnumerable<WeatherForecast>>
{ }
