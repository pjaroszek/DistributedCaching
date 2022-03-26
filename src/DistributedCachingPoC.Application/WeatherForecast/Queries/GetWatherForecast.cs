
namespace DistributedCachingPoC.Application.WeatherForecast.Queries;

using MediatR;
using Models;

public sealed record GetWatherForecast : IRequest<IEnumerable<WeatherForecast>>
{ }
