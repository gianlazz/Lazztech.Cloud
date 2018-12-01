﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HackathonManager.DTO;
using Lazztech.ObsidianPresences.ClientFacade.Data;

namespace Lazztech.ObsidianPresences.ClientFacade.Pages.Events.Admin.Mentors
{
    public class EditModel : PageModel
    {
        private readonly Lazztech.ObsidianPresences.ClientFacade.Data.ApplicationDbContext _context;

        public EditModel(Lazztech.ObsidianPresences.ClientFacade.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Mentor Mentor { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Mentor = await _context.Mentor.FirstOrDefaultAsync(m => m.Id == id);

            if (Mentor == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Mentor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MentorExists(Mentor.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MentorExists(Guid id)
        {
            return _context.Mentor.Any(e => e.Id == id);
        }
    }
}
