﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HackathonManager;
using Lazztech.ObsidianPresences.ClientFacade.Data;
using HackathonManager.RepositoryPattern;

namespace Lazztech.ObsidianPresences.ClientFacade.Pages.Events.Admin.Judges
{
    public class EditModel : PageModel
    {
        //private readonly Lazztech.ObsidianPresences.ClientFacade.Data.ApplicationDbContext _context;

        //public EditModel(Lazztech.ObsidianPresences.ClientFacade.Data.ApplicationDbContext context)
        //{
        //    _context = context;
        //}
        private IRepository _repo = Startup.DbRepo;

        [BindProperty]
        public Judge Judge { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Judge = await _context.Judge.FirstOrDefaultAsync(m => m.Id == id);
            Judge = _repo.All<Judge>().FirstOrDefault(m => m.Id == id);

            if (Judge == null)
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

            //_context.Attach(Judge).State = EntityState.Modified;

            try
            {
                _repo.Delete<Judge>(x => x.Id == Judge.Id);
                _repo.Add<Judge>(Judge);
                //await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JudgeExists(Judge.Id))
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

        private bool JudgeExists(Guid id)
        {
            return _repo.All<Judge>().Any(x => x.Id == id);
            //return _context.Judge.Any(e => e.Id == id);
        }
    }
}