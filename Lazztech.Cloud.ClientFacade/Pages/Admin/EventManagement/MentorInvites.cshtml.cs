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
            NewInvite.Event = NewMentor.Event;

            string domainName = Request.HttpContext.Request.GetDisplayUrl().Replace(Request.Path, String.Empty);
            NewInvite.InviteLink = $"{domainName}/Events/Invites?Id=" + $"{NewInvite.MentorInviteId}";

            if (NewMentor.PhoneNumber != null)
                _sms.SendSms(NewMentor.PhoneNumber, $"You've been invited to mentor at {NewInvite.Event.Organization.Name} {NewInvite.Event.Name}!" +
                    $" Please follow the link to sign up: {NewInvite.InviteLink}");

            _context.MentorInvites.Add(NewInvite);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}