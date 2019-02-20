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

            var mentors = _context.Mentors
                .Where(x => x.EventId == eventId)
                .OrderBy(z => z.IsAvailable ? 0 : 1);
            var invites = _context.MentorInvites.Where(x => x.Mentor.EventId == eventId);
            var incompletedInvites = invites.Where(x => x.Accepted == false);

            foreach (var mentor in mentors)
            {
                if (!incompletedInvites.Any(x => x.MentorId == mentor.MentorId))
                    Mentors.Add(mentor);
            }

            return Page();
        }
    }
}