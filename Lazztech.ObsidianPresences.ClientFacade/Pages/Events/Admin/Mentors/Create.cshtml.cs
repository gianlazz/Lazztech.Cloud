using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackathonManager.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lazztech.ObsidianPresences.ClientFacade.Pages.Events.Admin.Mentors
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Mentor Mentor { get; set; }

        public void OnGet()
        {
            Mentor = new Mentor();
        }

        public void OnPost(FormCollection collection)
        {

        }
    }
}