using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HackathonManager.PocoModels;
using Lazztech.ObsidianPresences.ClientFacade.Data;

namespace Lazztech.ObsidianPresences.ClientFacade.Pages.Events.Admin.Teams
{
    public class EditModel : PageModel
    {
        private readonly Lazztech.ObsidianPresences.ClientFacade.Data.ApplicationDbContext _context;

        public EditModel(Lazztech.ObsidianPresences.ClientFacade.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Team Team { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Team = await _context.Team.FirstOrDefaultAsync(m => m.Id == id);

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

            _context.Attach(Team).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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
            return _context.Team.Any(e => e.Id == id);
        }
    }
}
