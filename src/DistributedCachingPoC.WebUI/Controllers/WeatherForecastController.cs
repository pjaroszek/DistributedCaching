namespace DistributedCachingPoC.WebUI.Controllers;

using DistributedCachingPoC.Application.WeatherForecast.Queries;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> logger;
    private readonly IMediator mediator;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator) =>
        (this.logger, this.mediator) = (logger, mediator);

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IActionResult> Get()
    {
        var command = new GetWatherForecast();

        this.logger.LogInformation("{Command}", command);

        var result = await mediator.Send(command, CancellationToken.None);

        return await Task.FromResult(Ok(result)).ConfigureAwait(false);
    }
}
