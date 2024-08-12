using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiTasks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateController : ControllerBase
    {
        [HttpPost]

        public async Task<ActionResult> Update([FromForm] Image Img, [FromForm] string Owner)
        {
            IFormFile? file = Img.File;
            if (file == null || file.Length == 0)
            {
                return BadRequest("File does not exist");
            }

            var extension = Path.GetExtension(file.FileName).ToLower();
            if (string.IsNullOrEmpty(extension) || extension != ".jpg")
            {
                return BadRequest("Invalid extension, Please select a jpg or a jpeg image");
            }
            try
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                var filePath = Path.Combine(path, fileName);

                if (System.IO.File.Exists(filePath))
                {
                    var metaDataPath = Path.Combine(path, $"{Path.GetFileNameWithoutExtension(fileName)}.json");
                    var metaDataJson = System.IO.File.ReadAllText(metaDataPath);
                    var metaData = Newtonsoft.Json.JsonConvert.DeserializeObject<Info>(metaDataJson);
                    if (metaData.Owner == Owner)
                    {
                        metaData.LastModificationTime = DateTime.Now;
                        await System.IO.File.WriteAllTextAsync(metaDataPath, Newtonsoft.Json.JsonConvert.SerializeObject(metaData));
                        return Ok("Updated");
                    }

                    return BadRequest("Forbidden, You are not allowed to access someone else's file");
                }
                return BadRequest("File has not been stored on the server previously");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error" + ex);
            }

        }
    }
}
