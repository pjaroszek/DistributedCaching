namespace DistributedCachingPoC.WebUI.Controllers;

using Microsoft.AspNetCore.Mvc;

public sealed class DefaultController : Controller
{
    [ApiExplorerSettings(IgnoreApi = true), HttpGet, Route("")]
    public RedirectResult Index() => Redirect("swagger/index.html");
}
