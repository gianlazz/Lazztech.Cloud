using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace Lazztech.Cloud.ClientFacade.Pages.Events.Admin
{
    public class IndexModel : PageModel
    {
        public List<Mentor> Mentors { get; set; }
        public List<Judge> Judges { get; set; }
        public List<Team> Teams { get; set; }
        public List<SmsDto> Messages { get; set; }

        private IRepository _repo = Startup.DbRepo;

        public IndexModel()
        {
            Mentors = new List<Mentor>();
            Judges = new List<Judge>();
            Teams = new List<Team>();
            Messages = new List<SmsDto>();
        }

        public void OnGet()
        {
            Mentors.AddRange(_repo.All<Mentor>().ToList());
            Judges.AddRange(_repo.All<Judge>().ToList());
            Teams.AddRange(_repo.All<Team>().ToList());
            Messages = _repo.All<SmsDto>().Take(10).ToList();
        }
    }
}