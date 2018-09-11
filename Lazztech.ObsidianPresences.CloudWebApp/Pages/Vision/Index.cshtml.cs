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

namespace Lazztech.ObsidianPresences.CloudWebApp.Pages.Vision
{
    public class IndexModel : PageModel
    {
        public List<Snapshot> Snapshots { get; set; }

        public void OnGet()
        {
            Snapshots = new List<Snapshot>();

            var snaps = CallSnapsEndpoint();
            Snapshots = snaps.Result;
        }

        //Hosted web API REST Service base url
        private string baseurl = "http://lazztech.obsidianpresences.vision.microservice.webapi/";

        private async Task<List<Snapshot>> CallSnapsEndpoint()
        {
            List<Snapshot> snaps = new List<Snapshot>();

            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/unidentifiedsnapshots");

                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list
                    snaps = JsonConvert.DeserializeObject<List<Snapshot>>(EmpResponse);
                }

                return snaps;
            }
        }
    }
}