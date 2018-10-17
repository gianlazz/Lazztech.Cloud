using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lazztech.ObsidianPresences.Vision.Microservice.Domain;
using Lazztech.ObsidianPresences.Vision.Microservice.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Lazztech.ObsidianPresences.Vision.Microservice.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddNewPersonController : ControllerBase
    {
        // GET: api/AddNewPerson
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/AddNewPerson/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/AddNewPerson
        [HttpPost]
        public void Post([FromBody] string base64Image, string name)
        {
            var snapshot = new Snapshot();

            var dir = FacialRecognitionManager.knownPath;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            //var jsonDirs = Directory.GetFiles(dir).Where(x => x.EndsWith(".json"));
            //var json = System.IO.File.ReadAllText(jsonDir);
            //var snapshotObject = JsonConvert.DeserializeObject(json);
            //var snapshot = JsonConvert.DeserializeObject<Snapshot>(json);
            //var imageFound = System.IO.File.Exists(snapshot.ImageDir);
            var imageExtension = base64Image.TrimStart("data:image/".ToArray()).Split(';').First();
            var imageBytes = Convert.FromBase64String(base64Image);
            System.IO.File.WriteAllBytesAsync(dir + $"{name}.{imageExtension}", imageBytes);

            //snapshot.ImageDir = $"data:image/{imageExtension};base64, {imageBase64}";
        }

        // PUT: api/AddNewPerson/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
