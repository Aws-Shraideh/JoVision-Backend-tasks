using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiTasks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteController : ControllerBase
    {
        [HttpGet]
        public ActionResult Delete([FromQuery] string fileName, [FromQuery] string fileOwner)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(fileOwner))
            {
                return BadRequest("FileName and FileOwner are required.");
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            var filePath = Path.Combine(path, fileName + ".jpg");
            var metaDataFilePath = Path.Combine(path, $"{Path.GetFileNameWithoutExtension(fileName)}.json");

            if (!System.IO.File.Exists(filePath))
            {
                return BadRequest("File does not exist.");
            }

            try
            {
                var metaDataJson = System.IO.File.ReadAllText(metaDataFilePath);
                var metaData = Newtonsoft.Json.JsonConvert.DeserializeObject<Info>(metaDataJson);

                if (metaData.Owner != fileOwner)
                {
                    return Forbid("Owner name does not match.");
                }

                System.IO.File.Delete(filePath);
                System.IO.File.Delete(metaDataFilePath);

                return Ok("Deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

}
