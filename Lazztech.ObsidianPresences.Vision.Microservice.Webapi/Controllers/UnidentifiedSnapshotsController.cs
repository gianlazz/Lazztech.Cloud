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
    public class UnidentifiedSnapshotsController : ControllerBase
    {
        public List<Snapshot> Snapshots { get; set; }

        // GET: api/UnidentifiedSnapshots
        [HttpGet]
        public ActionResult<List<Snapshot>> Get()
        {
            Snapshots = new List<Snapshot>();

            var dir = FacialRecognitionManager.unknownJsonsPath;

            var jsonDirs = Directory.GetFiles(dir).Where(x => x.EndsWith(".json"));
            foreach (var jsonDir in jsonDirs)
            {
                var json = System.IO.File.ReadAllText(jsonDir);
                var snapshotObject = JsonConvert.DeserializeObject(json);
                var snapshot = JsonConvert.DeserializeObject<Snapshot>(json);
                var imageFound = System.IO.File.Exists(snapshot.ImageDir);
                var imageBytes = System.IO.File.ReadAllBytes(snapshot.ImageDir);
                var imageBase64 = Convert.ToBase64String(imageBytes);
                var imageExtension = snapshot.ImageDir.Split('.').Last();
                snapshot.ImageDir = $"data:image/{imageExtension};base64, {imageBase64}";
                Snapshots.Add(snapshot);
            }

            Snapshots.RemoveAll(x => x.Status != Snapshot.SnapshotStatus.unknown_person);

            return Snapshots;
        }

        // GET: api/UnidentifiedSnapshots/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/UnidentifiedSnapshots
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/UnidentifiedSnapshots/5
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
