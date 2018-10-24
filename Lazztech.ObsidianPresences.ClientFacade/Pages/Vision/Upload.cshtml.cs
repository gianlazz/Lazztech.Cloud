using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Lazztech.ObsidianPresences.Vision.Microservice.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Lazztech.ObsidianPresences.ClientFacade.Pages.Vision
{
    public class UploadModel : PageModel
    {
        public string Name { get; set; }
        public IFormFile Photo { get; set; }
        public string ImageBase64 { get; private set; }

        public bool ConnectedToServices = false;

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await UploadPhoto();
            //Message = "New customer created successfully!";

            return RedirectToPage("./Known");
        }

        private async Task UploadPhoto()
        {
            using (var ms = new MemoryStream())
            {
                await Photo.CopyToAsync(ms);
                var imageBytes = ms.ToArray();
                ImageBase64 = Convert.ToBase64String(imageBytes);
            }

            try
            {
                using (var client = new HttpClient())
                {
                    //Passing service base url
                    client.BaseAddress = new Uri(StaticStrings.VisionWebapiService);

                    client.DefaultRequestHeaders.Clear();
                    //Define request data format
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var dictionary = new List<string>();
                    dictionary.Add(ImageBase64);
                    dictionary.Add("ExampleName");
                    var json = JsonConvert.SerializeObject(dictionary);
                    var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    var Res = await client.PostAsync("api/AddNewPerson", httpContent);

                    //Checking the response is successful or not which is sent using HttpClient
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Employee list
                        ConnectedToServices = true;
                    }

                    //ConnectedToServices = true;
                }
            }
            catch (Exception)
            {

                throw;
            }

            //HERE I WOULD BASE64 ENCODE THE FILESTREAM AND POST IT TO THE VISION SERVICE'S NEW ADDNEWPERSON API CONTROLLER
        }
    }

    public class Customer
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        public string PhotePath { get; set; }

        [Required]
        [NotMapped]
        public IFormFile Phote { get; set; }
    }
}