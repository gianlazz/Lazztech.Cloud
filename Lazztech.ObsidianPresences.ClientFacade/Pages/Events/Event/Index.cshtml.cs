using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackathonManager.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lazztech.ObsidianPresences.ClientFacade.Pages.Events.Event
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public List<Mentor> Mentors { get; set; }

        public IndexModel()
        {
            Mentors = new List<Mentor>();
        }

        public void OnGet()
        {

        }
    }
}