using Microsoft.AspNetCore.Mvc;

namespace WebApiTasks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GreeterController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get([FromQuery] string name = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Ok("Hello anonymous");
            }
            return Ok($"Hello {name}");
        }
    }
}
