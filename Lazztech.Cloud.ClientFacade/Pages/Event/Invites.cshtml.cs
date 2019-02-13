using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Lazztech.Cloud.ClientFacade.Data;
using Lazztech.Cloud.Vision.Domain;
using Lazztech.Events.Dal.Dao;
using Lazztech.Events.Dto.Interfaces;
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
        private readonly IFileServices _fileService;
        private readonly ISmsService _sms;

        public InvitesModel(ApplicationDbContext applicationDbContext, IFileServices fileServices, ISmsService sms)
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
    }
}