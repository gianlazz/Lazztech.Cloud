using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HackathonManager.PocoModels;
using Lazztech.ObsidianPresences.ClientFacade.Data;
using HackathonManager.RepositoryPattern;

namespace Lazztech.ObsidianPresences.ClientFacade.Pages.Events.Admin.Teams
{
    public class IndexModel : PageModel
    {
        //private readonly Lazztech.ObsidianPresences.ClientFacade.Data.ApplicationDbContext _context;

        //public IndexModel(Lazztech.ObsidianPresences.ClientFacade.Data.ApplicationDbContext context)
        //{
        //    _context = context;
        //}
        private IRepository _repo = Startup.DbRepo;

        public IList<Team> Team { get;set; }

        public async Task OnGetAsync()
        {
            Team = _repo.All<Team>().ToList();
            //Team = await _context.Team.ToListAsync();
        }
    }
}
