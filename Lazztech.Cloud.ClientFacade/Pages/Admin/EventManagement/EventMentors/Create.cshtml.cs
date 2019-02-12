using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Lazztech.Cloud.ClientFacade.Data;
using Lazztech.Events.Dal.Dao;

namespace Lazztech.Cloud.ClientFacade.Pages.Admin.EventManagement.EventMentors
{
    public class CreateModel : PageModel
    {
        private readonly Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext _context;

        public CreateModel(Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId");
        ViewData["MentorId"] = new SelectList(_context.Mentors, "MentorId", "MentorId");
            return Page();
        }

        [BindProperty]
        public EventMentor EventMentor { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.EventMentors.Add(EventMentor);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}