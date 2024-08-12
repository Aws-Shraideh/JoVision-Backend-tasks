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
    }
}
   
