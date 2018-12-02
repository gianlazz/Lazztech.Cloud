using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HackathonManager.PocoModels;
using Lazztech.ObsidianPresences.ClientFacade.Data;
using HackathonManager.RepositoryPattern;

namespace Lazztech.ObsidianPresences.ClientFacade.Pages.Events.Admin.Teams
{
    public class CreateModel : PageModel
    {
        //private readonly Lazztech.ObsidianPresences.ClientFacade.Data.ApplicationDbContext _context;

        //public CreateModel(Lazztech.ObsidianPresences.ClientFacade.Data.ApplicationDbContext context)
        //{
        //    _context = context;
        //}
        private IRepository _repo = Startup.DbRepo;

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Team Team { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //_context.Team.Add(Team);
            //await _context.SaveChangesAsync();
            _repo.Add<Team>(Team);

            return RedirectToPage("./Index");
        }
    }
}