﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HackathonManager.DTO;
using Lazztech.ObsidianPresences.ClientFacade.Data;

namespace Lazztech.ObsidianPresences.ClientFacade.Pages.Events.Admin.Mentors
{
    public class IndexModel : PageModel
    {
        private readonly Lazztech.ObsidianPresences.ClientFacade.Data.ApplicationDbContext _context;

        public IndexModel(Lazztech.ObsidianPresences.ClientFacade.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Mentor> Mentor { get;set; }

        public async Task OnGetAsync()
        {
            Mentor = await _context.Mentor.ToListAsync();
        }
    }
}
