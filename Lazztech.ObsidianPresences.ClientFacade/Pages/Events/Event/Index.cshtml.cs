using Lazztech.Events.Dto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace Lazztech.Cloud.ClientFacade.Pages.Events.Event
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public List<Mentor> Mentors { get; set; }
        public string Message { get; set; }

        public IndexModel()
        {
            Mentors = new List<Mentor>();
        }

        public void OnGet(string message)
        {
            Message = message;

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