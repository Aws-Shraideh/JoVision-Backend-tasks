using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace WebApiTasks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateController : ControllerBase
    {

        [HttpPost]
        public async Task<ActionResult> Create([FromForm] Image Img, [FromForm] string owner)
        {
            IFormFile? file = Img.File;
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file selected");
            }

            var extension = Path.GetExtension(file.FileName).ToLower();
            if (string.IsNullOrEmpty(extension) || (extension != ".jpg" ))
            {
                return BadRequest("Invalid file type");
            }

            var fileName = Path.GetFileName(file.FileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var filePath = Path.Combine(path, fileName);

            if (System.IO.File.Exists(filePath))
            {
                return BadRequest("File with the same name already exists.");
            }

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var metaData = new Info
                {
                    Owner = owner,
                    CreationTime = DateTime.Now,
                    LastModificationTime = DateTime.Now
                };

                var metadataFilePath = Path.Combine(path, $"{Path.GetFileNameWithoutExtension(fileName)}.json");
                await System.IO.File.WriteAllTextAsync(metadataFilePath, Newtonsoft.Json.JsonConvert.SerializeObject(metaData));

                return Ok("Created");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /*

        [HttpDelete("Delete")]
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
        */

        /*
        [HttpPost("Update")]

        public async Task<ActionResult> Update([FromForm] Image Img, [FromForm] string Owner)
        {
            IFormFile? file = Img.File;
            if (file == null || file.Length == 0)
            {
                return BadRequest("File does not exist");
            }

            var extension = Path.GetExtension(file.FileName).ToLower();
            if (string.IsNullOrEmpty(extension) || (extension != ".jpeg" && extension != ".jpg"))
            {
                return BadRequest("Invalid extension, Please select a jpg or a jpeg image");
            }

            var fileName = Path.GetFileName(file.FileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(),"Uploads");
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

                return Forbid("You are not allowed to access someone else's file");
            }
            return BadRequest("File has not been stored on the server previously");
        }
        */
    }
}
   
