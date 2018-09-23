﻿using System;
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
    public class ProcessedSnapshotsController : ControllerBase
    {
        public List<Snapshot> Snapshots { get; set; }

        // GET: api/ProcessedSnapshots
        [HttpGet]
        public ActionResult<List<Snapshot>> Get()
        {
            Snapshots = new List<Snapshot>();

            var dir = FacialRecognitionManager.unknownJsonsPath;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

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

            return Snapshots;
        }

        // GET: api/ProcessedSnapshots/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ProcessedSnapshots
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/ProcessedSnapshots/5
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
