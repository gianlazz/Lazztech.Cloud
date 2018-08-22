using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lazztech.ObsidianPresences.Vision.Microservice.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Lazztech.ObsidianPresences.CloudWebApp.Pages
{
    public class VisionModel : PageModel
    {
        //public List<Snapshot> Snapshots => new List<Snapshot>();
        public List<Snapshot> Snapshots { get; set; }
        // public List<Snapshot> Snaps { get; set; }

        public void OnGet()
        {
            Snapshots = new List<Snapshot>();

            var resultsDir = @"/face/results/";
            var dirExists = Directory.Exists(resultsDir);

            var faceFolders = Directory.GetDirectories("/face/");
            var knownImageDirs = Directory.GetFiles("/face/known/");
            var unknownImageDirs = Directory.GetFiles("/face/unknown");

            var jsonDirs = Directory.GetFiles(resultsDir).Where(x => x.EndsWith(".json"));
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
        }
    }
}
