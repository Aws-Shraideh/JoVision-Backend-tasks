using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices.JavaScript;

namespace WebApiTasks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferOwnershipController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] string OldOwner, [FromQuery] string NewOwner)
        {
            if(OldOwner == NewOwner)
            {
                return BadRequest("Both owners are the same");
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            var files = Directory.GetFiles(path);
            string? fileMetaDataJson = null;
            var OldOwnerCount = 0;
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
                                OldOwnerCount++;
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
                if (JsonInfo.Count() == 0) 
                {
                    return BadRequest($"Neither {OldOwner} or {NewOwner} have any files");
                }
                if (OldOwnerCount == 0) 
                {
                    return Ok(new { Alert = $"{OldOwner} has no files", Files = JsonInfo });
                }
                if (OldOwner.ToLower() == NewOwner.ToLower()) 
                {
                    return Ok(new { Alert = $"{OldOwner} no longer has any files", Files = JsonInfo });
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
