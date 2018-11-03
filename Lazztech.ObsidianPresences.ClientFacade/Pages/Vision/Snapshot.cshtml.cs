using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Lazztech.ObsidianPresences.Vision.Microservice.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Lazztech.ObsidianPresences.ClientFacade.Pages.Vision
{
    public class SnapshotModel : PageModel
    {
        public Snapshot Snap { get; set; }

        private string VisionServiceUrl => StaticStrings.VisionWebapiService;

        public bool ConnectedToServices = false;

        public void OnGet(string id)
        {
            var snaps = GetSnapshotFromSnapsById(id);
            Snap = snaps.Result;
        }

        private async Task<Snapshot> GetSnapshotFromSnapsById(string id)
        {
            List<Snapshot> snaps = new List<Snapshot>();

            try
            {
                using (var client = new HttpClient())
                {
                    //Passing service base url
                    client.BaseAddress = new Uri(VisionServiceUrl);

                    client.DefaultRequestHeaders.Clear();
                    //Define request data format
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                    HttpResponseMessage Res = await client.GetAsync("api/ProcessedSnapshots");

                    //Checking the response is successful or not which is sent using HttpClient
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Employee list
                        snaps = JsonConvert.DeserializeObject<List<Snapshot>>(EmpResponse);
                        ConnectedToServices = true;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            //var result = snaps.Where(x => x.GuidId.ToString() == Guid.Parse(id).ToString()).FirstOrDefault();

            Snapshot result = new Snapshot();
            var guid = Guid.Parse(id);
            foreach (var snap in snaps)
            {
                //if (snap.ImageName == "ad84274b-68f6-441d-bcf1-6f7690b1ccbe.jpg")
                //    result = snap;
                if (snap.GuidId == guid)
                    result = snap;
            }

            return result;
        }
    }
}