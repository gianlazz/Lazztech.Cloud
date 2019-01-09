using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HackathonManager.PocoModels;
using Lazztech.Cloud.ClientFacade.Data;
using HackathonManager.RepositoryPattern;

namespace Lazztech.Cloud.ClientFacade.Pages.Events.Admin.Teams
{
    public class DeleteModel : PageModel
    {
        //private readonly Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext _context;

        //public DeleteModel(Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        private IRepository _repo = Startup.DbRepo;

        [BindProperty]
        public Team Team { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Team = await _context.Team.FirstOrDefaultAsync(m => m.Id == id);
            Team = _repo.All<Team>().FirstOrDefault(m => m.Id == id);

            if (Team == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Team = await _context.Team.FindAsync(id);
            Team = _repo.All<Team>().FirstOrDefault(x => x.Id == id);

            if (Team != null)
            {
                //_context.Team.Remove(Team);
                //await _context.SaveChangesAsync();
                _repo.Delete<Team>(Team);
            }

            return RedirectToPage("./Index");
        }
    }
}
