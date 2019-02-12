using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lazztech.Cloud.ClientFacade.Data;
using Lazztech.Events.Dal.Dao;

namespace Lazztech.Cloud.ClientFacade.Pages.Admin.EventManagement.EventMentors
{
    public class DeleteModel : PageModel
    {
        private readonly Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext _context;

        public DeleteModel(Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public EventMentor EventMentor { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            EventMentor = await _context.EventMentors
                .Include(e => e.Event)
                .Include(e => e.Mentor).FirstOrDefaultAsync(m => m.EventId == id);

            if (EventMentor == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            EventMentor = await _context.EventMentors.FindAsync(id);

            if (EventMentor != null)
            {
                _context.EventMentors.Remove(EventMentor);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
