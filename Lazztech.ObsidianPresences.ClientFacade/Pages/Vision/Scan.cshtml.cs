using Lazztech.ObsidianPresences.Vision.Microservice.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Lazztech.ObsidianPresences.ClientFacade.Pages.Vision
{
    public class ScanModel : PageModel
    {
        [BindProperty]
        public IFormFile Photo { get; set; }
        public string Id { get; set; }

        public string ImageBase64 { get; set; }
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

            return RedirectToPage($"./Snapshot/", new { id = Id });
            //return Page();
        }

        private async Task UploadPhoto()
        {
            using (var ms = new MemoryStream())
            {
                await Photo.CopyToAsync(ms);
                var extension = Path.GetExtension(Photo.FileName).Replace(".", string.Empty);
                var imageBytes = ms.ToArray();
                var prefix = $"data:image/{extension};base64,";
                ImageBase64 = prefix + Convert.ToBase64String(imageBytes);
            }

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticStrings.VisionWebapiService);

                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var dictionary = new Dictionary<string, string>();
                    dictionary.Add("Base64Image", ImageBase64);
                    var json = JsonConvert.SerializeObject(dictionary, Formatting.Indented);
                    var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    var Res = await client.PostAsync("api/ScanNewImage", httpContent);

                    if (Res.IsSuccessStatusCode)
                    {
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                        var responseDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(EmpResponse);
                        responseDictionary.TryGetValue("id", out string result);
                        Id = result;
                        ConnectedToServices = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}