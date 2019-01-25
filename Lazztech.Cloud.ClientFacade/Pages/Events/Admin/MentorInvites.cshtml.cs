using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Lazztech.Standard.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lazztech.Cloud.ClientFacade.Pages.Events.Admin
{
    public class MentorInvitesModel : PageModel
    {
        public List<MentorInvite> Invites { get; set; }
        public Mentor NewMentor { get; set; }

        private readonly IRepository _repo;
        private readonly ISmsService _sms;
        private readonly IEmailService _email;

        public MentorInvitesModel(IRepository repository, ISmsService sms, IEmailService email)
        {
            _repo = repository;
            _sms = sms;
            _email = email;
        }

        public void OnGet()
        {
            var invitesFromDb = _repo.All<MentorInvite>();
            Invites.AddRange(invitesFromDb);
        }

        public void OnPost(Mentor mentor)
        {
            var invite = new MentorInvite()
            {
                Mentor = mentor
            };

            var eventName = "CodeDay Seattle Eastside";
            var signUpLink = $"http://cloud.lazz.tech/Invites?Id=" + $"{invite.Id}";

            if (mentor.PhoneNumber != null)
                _sms.SendSms(mentor.PhoneNumber, $"You've been invited to mentor at {eventName}! Please follow the link to sign up: {signUpLink}");
            if (mentor.Email != null)
                _email.SendEmail(mentor.Email, "Mentor Registration", $"You've been invited to mentor at {eventName}! Please follow the link to sign up: {signUpLink}");

            _repo.Add<MentorInvite>(invite);
        }
    }
}