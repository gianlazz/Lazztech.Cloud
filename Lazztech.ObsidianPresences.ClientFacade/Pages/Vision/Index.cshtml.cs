using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Lazztech.ObsidianPresences.Vision.Microservice.Domain;
using Lazztech.ObsidianPresences.Vision.Microservice.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Lazztech.ObsidianPresences.ClientFacade.Pages.Vision
{
    public class IndexModel : PageModel
    {

        public List<Snapshot> Snapshots { get; set; }
        public bool ConnectedToServices = false;

        public void OnGet()
        {
            Snapshots = new List<Snapshot>();

            //var snaps = CallSnapsEndpoint();
            //Snapshots = snaps.Result;
            try
            {
                Snapshots = GetSnapshots();
                ConnectedToServices = true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private List<Snapshot> GetSnapshots()
        {
            var Snapshots = new List<Snapshot>();

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

            Snapshots.RemoveAll(x => x.Status != Snapshot.SnapshotStatus.unknown_person);

            return Snapshots;
        }

        //private async Task<List<Snapshot>> CallSnapsEndpoint()
        //{
        //    List<Snapshot> snaps = new List<Snapshot>();

        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            //Passing service base url
        //            client.BaseAddress = new Uri(baseurl);

        //            client.DefaultRequestHeaders.Clear();
        //            //Define request data format
        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //            //Sending request to find web api REST service resource GetAllEmployees using HttpClient
        //            HttpResponseMessage Res = await client.GetAsync("api/values");

        //            //Checking the response is successful or not which is sent using HttpClient
        //            if (Res.IsSuccessStatusCode)
        //            {
        //                //Storing the response details recieved from web api
        //                var EmpResponse = Res.Content.ReadAsStringAsync().Result;

        //                //Deserializing the response recieved from web api and storing into the Employee list
        //                snaps = JsonConvert.DeserializeObject<List<Snapshot>>(EmpResponse);
        //                ConnectedToServices = true;
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        //throw;
        //    }

        //    return snaps;
        //}
    }
}