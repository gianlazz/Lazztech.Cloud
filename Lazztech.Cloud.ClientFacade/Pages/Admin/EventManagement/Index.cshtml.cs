using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lazztech.Cloud.ClientFacade.Data;
using Lazztech.Events.Dal.Dao;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Lazztech.Cloud.ClientFacade.Pages.Admin.EventManagement
{
    public class IndexModel : PageModel
    {
        public int OrganizationCount { get; set; }
        public int EventCount { get; set; }
        public int MentorCount { get; set; }
        public int MentorInviteCount { get; set; }
        public int MentorRequestCount { get; private set; }
        public List<Lazztech.Events.Dal.Dao.Sms> Messages { get; set; }

        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task OnGetAsync()
        {
            OrganizationCount = await _context.Organizations.CountAsync();
            EventCount = await _context.Events.CountAsync();
            MentorCount = await _context.Mentors.CountAsync();
            MentorInviteCount = await _context.MentorInvites.CountAsync();
            MentorRequestCount = await _context.MentorRequests.CountAsync();
            Messages = await _context.SmsMessages.ToListAsync();
        }
    }
}