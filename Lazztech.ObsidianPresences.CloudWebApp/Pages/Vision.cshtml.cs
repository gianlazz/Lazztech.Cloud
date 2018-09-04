using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public void OnGet()
        {
            Snapshots = new List<Snapshot>();

            var snaps = CallSnapsEndpoint();
            Snapshots = snaps.Result;
        }

        //Hosted web API REST Service base url  
        //string Baseurl = "http://localhost:8080/";
        //string Baseurl = "http://localhost:50199/";
        //string Baseurl = "http://lazztechobsidianpresensevisionmicroservicewebapi:50199/";
        //string Baseurl = "http://lazztech.ObsidianPresences.vision.microservice.webapi:8080/";
        //string Baseurl = "http://172.20.0.3:5000/";
        //string Baseurl = "http://dockercompose18306792969269339587_lazztech.ObsidianPresences.vision.microservice.webapi_1:8080/";
        //string Baseurl = "http://c29edb6f84c8/";
        string Baseurl = "http://lazztech.obsidianpresences.vision.microservice.webapi/";
        //string Baseurl = "http://1bd392235d6e/";
        private async Task<List<Snapshot>> CallSnapsEndpoint()
        {
            List<Snapshot> snaps = new List<Snapshot>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/values");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    snaps = JsonConvert.DeserializeObject<List<Snapshot>>(EmpResponse);

                }
                //returning the employee list to view  
                return snaps;
            }
        }

        //public void OnGet()
        //{
        //    Snapshots = new List<Snapshot>();

        //    var resultsDir = @"/face/results/";
        //    var dirExists = Directory.Exists(resultsDir);

        //    var faceFolders = Directory.GetDirectories("/face/");
        //    var knownImageDirs = Directory.GetFiles("/face/known/");
        //    var unknownImageDirs = Directory.GetFiles("/face/unknown");

        //    var jsonDirs = Directory.GetFiles(resultsDir).Where(x => x.EndsWith(".json"));
        //    foreach (var jsonDir in jsonDirs)
        //    {
        //        var json = System.IO.File.ReadAllText(jsonDir);
        //        var snapshotObject = JsonConvert.DeserializeObject(json);
        //        var snapshot = JsonConvert.DeserializeObject<Snapshot>(json);
        //        var imageFound = System.IO.File.Exists(snapshot.ImageDir);
        //        var imageBytes = System.IO.File.ReadAllBytes(snapshot.ImageDir);
        //        var imageBase64 = Convert.ToBase64String(imageBytes);
        //        var imageExtension = snapshot.ImageDir.Split('.').Last();
        //        snapshot.ImageDir = $"data:image/{imageExtension};base64, {imageBase64}";
        //        Snapshots.Add(snapshot);
        //    }
        //}

        //public void HitRestPoint()
        //{
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("http://localhost:9000/");
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        // New code:
        //        HttpResponseMessage response = await client.GetAsync("api/products/1");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            Product product = await response.Content.ReadAsAsync<Product>();
        //            Console.WriteLine("{0}\t${1}\t{2}", product.Name, product.Price, product.Category);
        //        }
        //    }
        //}

    }
}

