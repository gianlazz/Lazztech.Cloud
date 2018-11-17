using Lazztech.ObsidianPresences.Vision.Microservice.Domain;
using Lazztech.ObsidianPresences.Vision.Microservice.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Lazztech.ObsidianPresences.ClientFacade.Pages.Vision
{
    public class UploadModel : PageModel
    {
        [BindProperty]
        [Required]
        public string Name { get; set; }

        [BindProperty]
        [Required]
        public IFormFile Photo { get; set; }

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

            return RedirectToPage("./Known");
        }

        private async Task UploadPhoto()
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    await Photo.CopyToAsync(ms);
                    var extension = Path.GetExtension(Photo.FileName).Replace(".", string.Empty);
                    var imageBytes = ms.ToArray();

                    var snapshot = new Snapshot();
                    var person = new Person() { Name = Name };
                    snapshot.People.Add(person);

                    var knownImagesDir = FacialRecognitionManager.knownPath;
                    if (!Directory.Exists(knownImagesDir))
                        Directory.CreateDirectory(knownImagesDir);

                    snapshot.ImageDir = knownImagesDir + $"{Name}.{extension}";
                    snapshot.ImageName = person.Name + "." + extension;
                    System.IO.File.WriteAllBytes(snapshot.ImageDir, imageBytes);

                    var knownJsonsDir = FacialRecognitionManager.knownJsonsPath;
                    if (!Directory.Exists(knownJsonsDir))
                        Directory.CreateDirectory(knownJsonsDir);
                    snapshot.DateTimeWhenCaptured = DateTime.Now;
                    var date = snapshot.DateTimeWhenCaptured.ToString("dd-MM-yyyy-hh-mm-ss-tt");
                    var jsonPath = $"{FacialRecognitionManager.knownJsonsPath}{date}_{snapshot.ImageName}_{snapshot.GetHashCode()}.json";
                    System.IO.File.WriteAllText(jsonPath, JsonConvert.SerializeObject(snapshot, Formatting.Indented));
                }

                ConnectedToServices = true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}