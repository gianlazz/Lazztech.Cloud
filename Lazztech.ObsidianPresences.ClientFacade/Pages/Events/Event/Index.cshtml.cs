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
            //var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();
            //string id = (string)hubContext.Clients.All.GetConnectionId().Result;

            var Db = Startup.DbRepo;

            //var db = Context.GetMLabsMongoDbRepo();
            Mentors = Db.All<Mentor>().Where(x => x.IsPresent == true).ToList().OrderBy(e => e.IsAvailable ? 0 : 1).ToList();
            //model = Db.All<Mentor>().Where(x => x.IsPresent == true).ToList();

            //mentorViewModel.PresentMentors = repo.All<Mentor>().Where(x => x.IsPresent == true).ToList();
            //mentorViewModel.AvailableMentors = repo.All<Mentor>().Where(x => x.IsAvailable == true).ToList();
        }
    }
}