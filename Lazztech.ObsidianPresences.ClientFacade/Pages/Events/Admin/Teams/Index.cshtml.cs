using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackathonManager.PocoModels;
using HackathonManager.RepositoryPattern;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lazztech.ObsidianPresences.ClientFacade.Pages.Events.Admin.Teams
{
    public class IndexModel : PageModel
    {
        public List<Team> Teams { get; set; }

        private IRepository _repo = Startup.DbRepo;

        public void OnGet()
        {
            Teams = _repo.All<Team>().ToList();
        }
    }
}