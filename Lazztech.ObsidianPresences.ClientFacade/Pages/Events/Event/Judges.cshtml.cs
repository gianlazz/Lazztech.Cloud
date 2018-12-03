using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackathonManager;
using HackathonManager.RepositoryPattern;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lazztech.ObsidianPresences.ClientFacade.Pages.Events.Event
{
    public class JudgesModel : PageModel
    {
        private IRepository _Db = Startup.DbRepo;


        public List<Judge> Judges { get; set; }

        public void OnGet()
        {
            Judges = _Db.All<Judge>().ToList();
        }
    }
}