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
    public class AddNewPersonController : Controller
    {
        // GET: api/AddNewPerson
        [HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/AddNewPerson/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/AddNewPerson
        [HttpPost]
        public JsonResult Post([FromBody]NewPersonModel newPersonModel)
        {
            var snapshot = new Snapshot();
            var name = newPersonModel.Name;
            var base64Image = newPersonModel.Base64Image;
            var person = new Person() { Name = name };
            snapshot.People.Add(person);

            var knownImagesDir = FacialRecognitionManager.knownPath;
            if (!Directory.Exists(knownImagesDir))
                Directory.CreateDirectory(knownImagesDir);

            var imageExtension = base64Image.TrimStart("data:image/".ToArray()).Split(';').First();
            var imageBytes = Convert.FromBase64String(base64Image.Substring(base64Image.IndexOf("base64,") + "base64,".Length));
            snapshot.ImageDir = knownImagesDir + $"{name}.{imageExtension}";
            snapshot.ImageName = person.Name + "." + imageExtension;
            System.IO.File.WriteAllBytesAsync(snapshot.ImageDir, imageBytes);

            var knownJsonsDir = FacialRecognitionManager.knownJsonsPath;
            if (!Directory.Exists(knownJsonsDir))
                Directory.CreateDirectory(knownJsonsDir);
            var date = snapshot.DateTimeWhenCaptured.ToString("dd-MM-yyyy-hh-mm-ss-tt");
            var jsonPath = $"{FacialRecognitionManager.knownJsonsPath}{date}_{snapshot.ImageName}_{snapshot.GetHashCode()}.json";
            System.IO.File.WriteAllText(jsonPath, JsonConvert.SerializeObject(snapshot, Formatting.Indented));

            //snapshot.ImageDir = $"data:image/{imageExtension};base64, {imageBase64}";
            return Json(new { success = true });
        }

        // PUT: api/AddNewPerson/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
