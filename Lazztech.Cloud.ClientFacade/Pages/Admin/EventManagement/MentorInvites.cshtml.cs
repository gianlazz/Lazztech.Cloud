using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lazztech.Cloud.ClientFacade.Data;
using Lazztech.Events.Dal.Dao;
using Lazztech.Events.Dto.Interfaces;
using Lazztech.Standard.Interfaces;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Lazztech.Cloud.ClientFacade.Pages.Admin.EventManagement
{
    public class MentorInvitesModel : PageModel
    {
        public List<MentorInvite> Invites { get; set; }

        [BindProperty]
        public Mentor NewMentor { get; set; }

        [BindProperty]
        public MentorInvite NewInvite { get; set; }

        private readonly ApplicationDbContext _context;
        private readonly ISmsService _sms;
        private readonly IEmailService _email;

        public MentorInvitesModel(ApplicationDbContext applicationDbContext, ISmsService sms, IEmailService email)
        {
            _context = applicationDbContext;
            _sms = sms;
            _email = email;
        }

        public async Task OnGetAsync()
        {
            Invites = await _context.MentorInvites.Include(x => x.Mentor).ToListAsync();

            ViewData["EventId"] = new SelectList(_context.Events.Include(x => x.Organization), "EventId", "Name");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            NewInvite.Mentor = NewMentor;
            NewInvite.Mentor.EventId = NewInvite.EventId;

            await _context.AddAsync(NewInvite);
            await _context.SaveChangesAsync();
            NewInvite = await _context.MentorInvites
                .Include(x => x.Event)
                .ThenInclude(x => x.Organization)
                .FirstOrDefaultAsync(x => x.MentorInviteId == NewInvite.MentorInviteId);

            string domainName = Request.HttpContext.Request.GetDisplayUrl().Replace(Request.Path, String.Empty);
            NewInvite.InviteLink = $"{domainName}/Event/Invites?Id=" + $"{NewInvite.MentorInviteId}";
            await _context.SaveChangesAsync();

            if (NewMentor.PhoneNumber != null)
                _sms.SendSms(NewMentor.PhoneNumber, $"You've been invited to mentor at {NewInvite.Event.Organization.Name}'s {NewInvite.Event.Name}!" +
                    $" Please follow the link to sign up: {NewInvite.InviteLink}");
            //if (NewMentor.Email != null)
            //    _email.SendEmail(NewMentor.Email, "Mentor Registration", $"You've been invited to mentor at {eventName}! Please follow the link to sign up: {signUpLink}");

            return RedirectToPage();
        }
    }
}