using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackathonManager;
using HackathonManager.DTO;
using HackathonManager.PocoModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lazztech.ObsidianPresences.ClientFacade.Pages.Events.Admin
{
    public class IndexModel : PageModel
    {
        public List<Mentor> Mentors { get; set; }
        public List<Judge> Judges { get; set; }
        public List<Team> Teams { get; set; }

        public IndexModel()
        {
            Mentors = new List<Mentor>();
            Judges = new List<Judge>();
            Teams = new List<Team>();
        }

        public void OnGet()
        {
            //var repo = MvcApplication.DbRepo;
            var repo = Startup.DbRepo;


            //overView.Mentors.AddRange(repo.All<Mentor>().Where(x => x.Event == "seattle-eastside"));
            try
            {
                Mentors.AddRange(repo.All<Mentor>().ToList());
                Judges.AddRange(repo.All<Judge>().ToList());
                Teams.AddRange(repo.All<Team>().ToList());
            }
            catch (Exception)
            {

            }
        }
    }
}