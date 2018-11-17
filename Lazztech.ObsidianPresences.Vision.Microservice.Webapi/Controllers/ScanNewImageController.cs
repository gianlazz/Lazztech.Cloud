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
        //[HttpPost]
        //public JsonResult Post([FromBody]NewImageModel newImage)
        //{
        //    var facialRecognition = new FacialRecognitionManager(new FaceRecognitionProcess(), new FaceDetectionProcess(), new FileServices());

        //    var unknownImagesDir = FacialRecognitionManager.unknownPath;

        //    var base64Image = newImage.Base64Image;
        //    var imageExtension = base64Image.TrimStart("data:image/".ToArray()).Split(';').First();
        //    var imageBytes = Convert.FromBase64String(base64Image.Substring(base64Image.IndexOf("base64,") + "base64,".Length));
        //    var imageName = Guid.NewGuid().ToString() + "." + imageExtension;

        //    var imageDirectory = unknownImagesDir + imageName;
        //    System.IO.File.WriteAllBytes(imageDirectory, imageBytes);

        //    var snapshot = facialRecognition.Process(imageDirectory);

        //    var unknownJsonsDir = FacialRecognitionManager.unknownJsonsPath;

        //    snapshot.DateTimeWhenCaptured = DateTime.Now;
        //    var date = snapshot.DateTimeWhenCaptured.ToString("dd-MM-yyyy-hh-mm-ss-tt");
        //    var jsonPath = $"{FacialRecognitionManager.unknownJsonsPath}{date}_{snapshot.ImageName}_{snapshot.GetHashCode()}.json";
        //    System.IO.File.WriteAllText(jsonPath, JsonConvert.SerializeObject(snapshot, Formatting.Indented));

        //    //System.IO.File.WriteAllText($"{FacialRecognitionManager.unknownJsonsPath}{dateExample}_{snapshot.ImageName}_{snapshot.GetHashCode()}.json", json);

        //    return Json(new { success = true, id = snapshot.GuidId });
        //}

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
