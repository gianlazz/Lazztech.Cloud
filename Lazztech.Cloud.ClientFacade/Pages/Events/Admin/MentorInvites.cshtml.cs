using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Lazztech.Standard.Interfaces;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace Lazztech.Cloud.ClientFacade.Pages.Events.Admin
{
    public class MentorInvitesModel : PageModel
    {
        public List<MentorInvite> Invites { get; set; }

        [BindProperty]
        public Mentor NewMentor { get; set; }

        [BindProperty]
        public MentorInvite NewInvite { get; set; }

        private readonly IDalHelper _repo;
        private readonly ISmsService _sms;
        private readonly IEmailService _email;

        public MentorInvitesModel(IDalHelper repository, ISmsService sms, IEmailService email)
        {
            _repo = repository;
            _sms = sms;
            _email = email;
            Invites = new List<MentorInvite>();
            NewMentor = new Mentor();
        }

        public void OnGet()
        {
            var invitesFromDb = _repo.All<MentorInvite>();
            Invites.AddRange(invitesFromDb);
        }

        public IActionResult OnPost()
        {
            var invite = new MentorInvite()
            {
                Mentor = NewMentor
            };

            var eventName = "CodeDay Seattle Eastside";
            string domainName = Request.HttpContext.Request.GetDisplayUrl().Replace(Request.Path, String.Empty);
            var inviteLink = $"{domainName}/Events/Invites?Id=" + $"{invite.Id}";
            invite.InviteLink = inviteLink;
            if (NewMentor.PhoneNumber != null)
                _sms.SendSms(NewMentor.PhoneNumber, $"You've been invited to mentor at {eventName}! Please follow the link to sign up: {inviteLink}");
            //if (NewMentor.Email != null)
            //    _email.SendEmail(NewMentor.Email, "Mentor Registration", $"You've been invited to mentor at {eventName}! Please follow the link to sign up: {signUpLink}");

            _repo.Add<MentorInvite>(invite);
            return RedirectToPage();
        }
    }
}