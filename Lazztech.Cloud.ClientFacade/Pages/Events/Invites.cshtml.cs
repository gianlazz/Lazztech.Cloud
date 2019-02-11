using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lazztech.Events.Domain;
using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Lazztech.Standard.Interfaces;
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
        [Required]
        public IFormFile Photo { get; set; }
        [BindProperty]
        public MentorInvite Invite { get; set; }

        private readonly IDalHelper _db;
        private readonly IFileService _fileService;
        private readonly ISmsService _sms;

        public InvitesModel(IDalHelper dal, IFileService fileService, ISmsService sms)
        {
            _db = dal;
            _fileService = fileService;
            _sms = sms;
        }

        public ActionResult OnGet(Guid? Id)
        {
            Invite = _db.Single<MentorInvite>(x => x.Id == Id);
            if (Invite == null)
                return NotFound();
            Mentor = Invite.Mentor;
            Invite.DateTimeWhenViewed = DateTime.Now;
            _db.Delete<MentorInvite>(x => x.Id == Id);
            _db.Add<MentorInvite>(Invite);

            return Page();
        }

        public async Task<ActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Invite.Accepted = true;

            _db.Delete<MentorInvite>(x => x.Id == Invite.Id);
            _db.Add<MentorInvite>(Invite);

            await UploadPhoto();
            _db.Add<Mentor>(Mentor);
            _sms.SendSms(Mentor.PhoneNumber, EventStrings.MentorRegistrationResponse(Mentor.FirstName));

            return RedirectToPage("/Events/Event/Index");
        }

        private async Task UploadPhoto()
        {
            using (var ms = new MemoryStream())
            {
                await Photo.CopyToAsync(ms);
                var extension = _fileService.GetExtension(Photo.FileName);
                var imageBytes = ms.ToArray();

                var directory = StaticStrings.dataDir;
                var fileName = Mentor.Id + extension;
                var imagePath = directory + fileName;
                Mentor.Image = imagePath;
                _fileService.WriteAllBytes(imagePath, imageBytes);
            }
        }
    }
}