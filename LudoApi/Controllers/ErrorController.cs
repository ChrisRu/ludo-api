using Microsoft.AspNetCore.Mvc;

namespace LudoApi.Controllers
{
    [Route("/{*url}")]
    [ApiController]
    public class ErrorController : Controller
    {
        [HttpGet]
        public ActionResult<string> Index()
        {
            return StatusCode(400, "This app is only made for use with SignalR");
        }
    }
}