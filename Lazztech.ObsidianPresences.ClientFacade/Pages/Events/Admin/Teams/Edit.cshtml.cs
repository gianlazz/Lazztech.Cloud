using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HackathonManager.PocoModels;
using Lazztech.Cloud.ClientFacade.Data;
using HackathonManager.RepositoryPattern;

namespace Lazztech.Cloud.ClientFacade.Pages.Events.Admin.Teams
{
    public class EditModel : PageModel
    {
        //private readonly Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext _context;

        //public EditModel(Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext context)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //_context.Attach(Team).State = EntityState.Modified;

            try
            {
                _repo.Delete<Team>(x => x.Id == Team.Id);
                _repo.Add<Team>(Team);
                //await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(Team.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TeamExists(Guid id)
        {
            return _repo.All<Team>().Any(x => x.Id == id);
            //return _context.Team.Any(e => e.Id == id);
        }
    }
}
