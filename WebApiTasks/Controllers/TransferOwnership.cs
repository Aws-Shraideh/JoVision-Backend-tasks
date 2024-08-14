using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiTasks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferOwnershipController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] string OldOwner, [FromQuery] string NewOwner)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            var files = Directory.GetFiles(path);
            string? fileMetaDataJson = null;
            List<Dictionary<string, string>> JsonInfo = new List<Dictionary<string, string>>();
            try
            {
                foreach (var file in files)
                {
                    if (Path.GetExtension(file) == ".json")
                    {
                        var metaDataPath = Path.Combine(path, Path.GetFileNameWithoutExtension(file) + ".json");
                        fileMetaDataJson = System.IO.File.ReadAllText(file);
                        var fileMetaData = Newtonsoft.Json.JsonConvert.DeserializeObject<Info>(fileMetaDataJson);
                        if (fileMetaData != null)
                        {
                            if (fileMetaData.Owner.ToLower() == OldOwner.ToLower())
                            {
                                fileMetaData.Owner = NewOwner;
                                await System.IO.File.WriteAllTextAsync(metaDataPath, Newtonsoft.Json.JsonConvert.SerializeObject(fileMetaData));
                            }
                        }
                        fileMetaDataJson = System.IO.File.ReadAllText(file);
                        var fileMetaDataList = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(fileMetaDataJson);
                        if (fileMetaDataList != null && fileMetaDataList["Owner"] == NewOwner)
                        {
                            JsonInfo.Add(fileMetaDataList);
                        }
                    }
                }
                return Ok(JsonInfo);

            } 
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
