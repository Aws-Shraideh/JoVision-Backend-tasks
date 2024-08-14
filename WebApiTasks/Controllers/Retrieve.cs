using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace WebApiTasks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RetrieveController : ControllerBase
    {
        [HttpGet]
        public ActionResult Retrieve([FromQuery] string? FileName = null, [FromQuery] string? FileOwner = null)
        {
            if (string.IsNullOrEmpty(FileName) || string.IsNullOrEmpty(FileOwner))
            {
                return BadRequest("FileName and FileOwner are required.");
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            var filePath = Path.Combine(path, FileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            var metaDataPath = Path.Combine(path, Path.GetFileNameWithoutExtension(FileName)+".json");
            if (!System.IO.File.Exists(metaDataPath))
            {
                return NotFound("Metadata not found.");
            }

            var metaDataJson = System.IO.File.ReadAllText(metaDataPath);
            var metaData = Newtonsoft.Json.JsonConvert.DeserializeObject<Info>(metaDataJson);

            if (metaData.Owner != FileOwner)
            {
                return BadRequest("Forbidden! You are trying to access someone else's files.");
            }

            try
            {
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "image/jpg",FileName);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
  

