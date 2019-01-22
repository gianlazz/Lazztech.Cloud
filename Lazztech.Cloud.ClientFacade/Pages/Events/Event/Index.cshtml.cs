using Lazztech.Events.Dto.Interfaces;
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
        [BindProperty]
        public string UniqueRequesteeId { get; set; }
        public string Message { get; set; }
        public string Alert { get; set; }

        private readonly IRepository _repo;

        public IndexModel(IRepository repository)
        {
            _repo = repository;
            Mentors = new List<Mentor>();
        }

        public void OnGet(string message, string alert)
        {
            Message = message;
            Alert = alert;

            if (Request.Cookies.ContainsKey(StaticStrings.eventUserIdCookieName))
                UniqueRequesteeId = Request.Cookies[StaticStrings.eventUserIdCookieName];

            //var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();
            //string id = (string)hubContext.Clients.All.GetConnectionId().Result;

            //var db = Context.GetMLabsMongoDbRepo();
            Mentors = _repo.All<Mentor>().Where(x => x.IsPresent == true).ToList().OrderBy(e => e.IsAvailable ? 0 : 1).ToList();
            //model = Db.All<Mentor>().Where(x => x.IsPresent == true).ToList();

            //mentorViewModel.PresentMentors = repo.All<Mentor>().Where(x => x.IsPresent == true).ToList();
            //mentorViewModel.AvailableMentors = repo.All<Mentor>().Where(x => x.IsAvailable == true).ToList();
        }
    }
}