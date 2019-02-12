using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lazztech.Cloud.ClientFacade.Data;
using Lazztech.Events.Dal.Dao;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Lazztech.Cloud.ClientFacade.Pages.Event
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public Organization Organization { get; set; }
        [BindProperty]
        public Lazztech.Events.Dal.Dao.Event Event { get; set; }
        [BindProperty]
        public List<Mentor> Mentors { get; set; }
        [BindProperty]
        public string UniqueRequesteeId { get; set; }
        public string Message { get; set; }
        public string Alert { get; set; }

        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;

            Mentors = new List<Mentor>();
        }

        public async Task<IActionResult> OnGetAsync(string message, string alert, int eventId)
        {
            Message = message;
            Alert = alert;

            Event = await _context.Events.Include(x => x.Organization).FirstOrDefaultAsync(x => x.EventId == eventId);
            if (Event == null)
                return NotFound();
            Organization = Event.Organization;

            if (Request.Cookies.ContainsKey(StaticStrings.eventUserIdCookieName))
                UniqueRequesteeId = Request.Cookies[StaticStrings.eventUserIdCookieName];

            Mentors = await _context.Mentors
                .Where(x => x.EventMentors.FirstOrDefault(y => y.EventId == eventId) != null)
                .OrderBy(z => z.IsAvailable ? 0 : 1).ToListAsync();

            return Page();
        }
    }
}