﻿using System;
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
    public class IndexModel : PageModel
    {
        public List<Snapshot> Snapshots => new List<Snapshot>();
        //public List<Snapshot> Snapshots { get; set; }
        // public List<Snapshot> Snaps { get; set; }

        public void OnGet()
        {
            //Snapshots = new List<Snapshot>();

            var resultsDir = @"/face/results/";
            var dirExists = Directory.Exists(resultsDir);

            var jsonDirs = Directory.GetFiles(resultsDir).Where(x => x.EndsWith(".json"));
            foreach (var jsonDir in jsonDirs)
            {
                var json = System.IO.File.ReadAllText(jsonDir);
                var snapshotObject = JsonConvert.DeserializeObject(json);
                var snapshot = JsonConvert.DeserializeObject<Snapshot>(json);;
                Snapshots.Add(snapshot);
            }
        }
    }
}
