using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lazztech.Cloud.ClientFacade.Pages.Events
{
    public class InvitesModel : PageModel
    {
        [BindProperty]
        public Mentor Mentor { get; set; }
        [BindProperty]
        public IFormFile Photo { get; set; }
        public MentorInvite Invite { get; set; }

        private readonly IRepository _repo;

        public InvitesModel(IRepository repository)
        {
            _repo = repository;
        }

        public ActionResult OnGet(Guid? Id)
        {
            Invite = _repo.Single<MentorInvite>(x => x.Id == Id);
            if (Invite == null)
                return NotFound();
            Mentor = Invite.Mentor;
            Invite.DateTimeWhenViewed = DateTime.Now;
            _repo.Delete<MentorInvite>(x => x.Id == Id);
            _repo.Add<MentorInvite>(Invite);

            return Page();
        }

        public async Task<ActionResult> OnPost()
        {
            Invite.Accepted = true;

            _repo.Delete<MentorInvite>(x => x.Id == Invite.Id);
            _repo.Add<MentorInvite>(Invite);

            await UploadPhoto();
            _repo.Add<Mentor>(Mentor);

            return RedirectToPage("./Event");
        }

        private async Task UploadPhoto()
        {
            throw new NotImplementedException();
        }
    }
}