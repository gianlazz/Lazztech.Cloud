using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lazztech.ObsidianPresences.Vision.Microservice.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lazztech.ObsidianPresences.ClientFacade.Pages.Vision
{
    public class UploadModel : PageModel
    {
        public Person Person { get; set; }
        public string Base64Photo { get; set; }

        public void OnGet()
        {

        }
    }
}