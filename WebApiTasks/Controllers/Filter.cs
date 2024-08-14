using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Linq;

namespace WebApiTasks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilterController : ControllerBase
    {
        public enum FilterEnum
        {
            ByModificationDate,
            ByCreationDateDescending,
            ByCreationDateAscending,
            ByOwner,
        }

        [HttpPost]
        public ActionResult Filter([FromForm] DateTime CreationDate, [FromForm] DateTime ModificationDate, [FromForm] string Owner, [FromForm] FilterEnum FilterType)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            string[] files = Directory.GetFiles(path);
            List<Dictionary<string, string>> metaData = new List<Dictionary<string, string>>();
            List<Dictionary<string, string>> filteredMetaData = new List<Dictionary<string, string>>();

            foreach (var file in files)
            {
                if (Path.GetExtension(file).ToLower() == ".json")
                {
                    var fileMetaData = System.IO.File.ReadAllText(file);
                    var deserializedData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(fileMetaData);
                    var fileName = Path.GetFileNameWithoutExtension(file);
                    if (deserializedData != null)
                    {
                        metaData.Add(deserializedData);

                    }
                }
            }

            if (FilterType == FilterEnum.ByModificationDate)
            {
                foreach (var dictionary in metaData)
                {
                    if (DateTime.TryParse(dictionary["LastModificationTime"], out DateTime parsedDate))
                    {
                        DateTime parsedDateUtc = parsedDate.ToUniversalTime();
                        DateTime modificationDateUtc = ModificationDate.ToUniversalTime();
                        int result = DateTime.Compare(parsedDateUtc, modificationDateUtc);
                        if (result < 0)
                        {
                            filteredMetaData.Add(dictionary);
                        }
                    }
                }
            }

            else if (FilterType == FilterEnum.ByOwner)
            {
                foreach (var dictionary in metaData)
                {
                    if (Owner.ToLower() == dictionary["Owner"].ToLower())
                    {
                        filteredMetaData.Add(dictionary);
                    }
                }
            }
            else if (FilterType == FilterEnum.ByCreationDateAscending)
            {
                foreach (var dictionary in metaData)
                {
                    if (DateTime.TryParse(dictionary["CreationTime"], out DateTime parsedDate))
                    {
                        DateTime parsedDateUtc = parsedDate.ToUniversalTime();
                        DateTime CreationDateUtc = CreationDate.ToUniversalTime();
                        int result = DateTime.Compare(parsedDateUtc, CreationDateUtc);
                        if (result > 0)
                        {
                            filteredMetaData.Add(dictionary);
                        }

                    }
                }
                filteredMetaData = filteredMetaData.OrderBy(dictionary => dictionary["CreationTime"]).ToList();
            }

            else if (FilterType == FilterEnum.ByCreationDateDescending)
            {
                foreach (var dictionary in metaData)
                {
                    if (DateTime.TryParse(dictionary["CreationTime"], out DateTime parsedDate))
                    {
                        DateTime parsedDateUtc = parsedDate.ToUniversalTime();
                        DateTime CreationDateUtc = CreationDate.ToUniversalTime();
                        int result = DateTime.Compare(parsedDateUtc, CreationDateUtc);
                        if (result > 0)
                        {
                            filteredMetaData.Add(dictionary);
                        }

                    }
                }
                filteredMetaData = filteredMetaData.OrderByDescending(dictionary => dictionary["CreationTime"]).ToList();
            }

            return Ok(filteredMetaData);
        }
    }
}

