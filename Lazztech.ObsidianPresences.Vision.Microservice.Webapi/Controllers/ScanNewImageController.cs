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
    public class ScanNewImageController : Controller
    {
        //// GET: api/ScanNewImage
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/ScanNewImage/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/ScanNewImage
        [HttpPost]
        public JsonResult Post([FromBody]NewImageModel newImage)
        {
            var snapshot = new Snapshot();
            var base64Image = newImage.Base64Image;

            var name = "";

            //PROCESS NEW ONES
            var Results = new List<Snapshot>();
            var facialRecognition = new FacialRecognitionManager(new FaceRecognitionProcess(), new FaceDetectionProcess(), new FileServices());
            Results = facialRecognition.Process();

            //WRITE OUT/PERSIST THE JSON RESULTS TO DISK
            foreach (var snap in Results)
            {
                var json = JsonConvert.SerializeObject(snap, Formatting.Indented);
                var dateExample = snap.DateTimeWhenCaptured.ToString("dd-MM-yyyy-hh-mm-ss-tt");
                if (snap.ImageDir.Contains("/known"))
                    System.IO.File.WriteAllText($"{FacialRecognitionManager.knownJsonsPath}{dateExample}_{snap.ImageName}_{snap.GetHashCode()}.json", json);
                if (snap.ImageDir.Contains("/unknown"))
                {
                    System.IO.File.WriteAllText($"{FacialRecognitionManager.unknownJsonsPath}{dateExample}_{snap.ImageName}_{snap.GetHashCode()}.json", json);
                    Console.WriteLine(json);
                }
            }

            var person = new Person() { Name = name };
            snapshot.People.Add(person);

            var unknownImagesDir = FacialRecognitionManager.unknownPath;
            if (!Directory.Exists(unknownImagesDir))
                Directory.CreateDirectory(unknownImagesDir);

            var imageExtension = base64Image.TrimStart("data:image/".ToArray()).Split(';').First();
            var imageBytes = Convert.FromBase64String(base64Image.Substring(base64Image.IndexOf("base64,") + "base64,".Length));
            snapshot.ImageDir = unknownImagesDir + $"{name}.{imageExtension}";
            snapshot.ImageName = person.Name + "." + imageExtension;
            System.IO.File.WriteAllBytesAsync(snapshot.ImageDir, imageBytes);

            var knownJsonsDir = FacialRecognitionManager.knownJsonsPath;
            if (!Directory.Exists(knownJsonsDir))
                Directory.CreateDirectory(knownJsonsDir);
            snapshot.DateTimeWhenCaptured = DateTime.Now;
            var date = snapshot.DateTimeWhenCaptured.ToString("dd-MM-yyyy-hh-mm-ss-tt");
            var jsonPath = $"{FacialRecognitionManager.knownJsonsPath}{date}_{snapshot.ImageName}_{snapshot.GetHashCode()}.json";
            System.IO.File.WriteAllText(jsonPath, JsonConvert.SerializeObject(snapshot, Formatting.Indented));

            return Json(new { success = true });
        }

        //// PUT: api/ScanNewImage/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }

    public class NewImageModel
    {
        public string Base64Image { get; set; }
    }
}
