using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lazztech.Cloud.ClientFacade.Data;
using Lazztech.Events.Dal.Dao;
using Lazztech.Events.Domain;
using Lazztech.Events.Dto.Interfaces;
using Lazztech.Standard.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Lazztech.Cloud.ClientFacade.Pages.Event
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

        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;
        private readonly ISmsService _sms;

        public InvitesModel(ApplicationDbContext applicationDbContext, IFileService fileServices, ISmsService sms)
        {
            _context = applicationDbContext;
            _fileService = fileServices;
            _sms = sms;
        }

        public async Task<ActionResult> OnGetAsync(int Id)
        {
            Invite = await _context.MentorInvites
                .Include(x => x.Mentor)
                .FirstOrDefaultAsync(x => x.MentorInviteId == Id);
            if (Invite == null)
                return NotFound();

            Mentor = Invite.Mentor;
            Invite.DateTimeWhenViewed = DateTime.Now;
            await _context.SaveChangesAsync();

            return Page();
        }

        public async Task<ActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            Invite.Accepted = true;
            await _context.SaveChangesAsync();

            await UploadPhoto();
            await _context.AddAsync(Mentor);
            await _context.SaveChangesAsync();

            _sms.SendSms(Mentor.PhoneNumber, EventStrings.MentorRegistrationResponse(Mentor.FirstName));

            return RedirectToPage("./");
        }

        private async Task UploadPhoto()
        {
            using (var ms = new MemoryStream())
            {
                await Photo.CopyToAsync(ms);
                var extension = _fileService.GetExtension(Photo.FileName);
                var imageBytes = ms.ToArray();

                var directory = StaticStrings.dataDir;
                var fileName = Mentor.MentorId + extension;
                var imagePath = directory + fileName;
                Mentor.Image = imagePath;
                _fileService.WriteAllBytes(imagePath, imageBytes);
            }
        }
    }
}