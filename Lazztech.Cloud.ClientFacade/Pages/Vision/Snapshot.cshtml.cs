using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Lazztech.Cloud.Vision.Domain;
using Lazztech.Cloud.Vision.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Lazztech.Cloud.ClientFacade.Pages.Vision
{
    public class SnapshotModel : PageModel
    {
        public Snapshot Snap { get; set; }

        public bool ConnectedToServices = false;

        public void OnGet(string id)
        {
            Snap = GetSnapshotFromSnapsById(id);
        }

        private Snapshot GetSnapshotFromSnapsById(string id)
        {
            List<Snapshot> snaps = new List<Snapshot>();

            var dir = FacialRecognitionManager.unknownJsonsPath;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            Snapshot result = new Snapshot();
            var jsonDirs = Directory.GetFiles(dir).Where(x => x.EndsWith(".json"));
            foreach (var jsonDir in jsonDirs)
            {
                var json = System.IO.File.ReadAllText(jsonDir);
                var snapshot = JsonConvert.DeserializeObject<Snapshot>(json);
                var imageBytes = System.IO.File.ReadAllBytes(snapshot.ImageDir);
                var imageBase64 = Convert.ToBase64String(imageBytes);
                var imageExtension = snapshot.ImageDir.Split('.').Last();
                snapshot.ImageDir = $"data:image/{imageExtension};base64, {imageBase64}";
                snaps.Add(snapshot);
            }

            result = snaps.Where(x => x.GuidId.ToString() == Guid.Parse(id).ToString()).FirstOrDefault();
            return result;
        }
    }
}