using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HackathonManager.DTO;
using Lazztech.ObsidianPresences.ClientFacade.Data;
using HackathonManager.RepositoryPattern;

namespace Lazztech.ObsidianPresences.ClientFacade.Pages.Events.Admin.Mentors
{
    public class DetailsModel : PageModel
    {
        //private readonly Lazztech.ObsidianPresences.ClientFacade.Data.ApplicationDbContext _context;

        //public DetailsModel(Lazztech.ObsidianPresences.ClientFacade.Data.ApplicationDbContext context)
        //{
        //    _context = context;
        //}
        private IRepository _repo = Startup.DbRepo;

        public Mentor Mentor { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Mentor = await _context.Mentor.FirstOrDefaultAsync(m => m.Id == id);
            Mentor = _repo.All<Mentor>().FirstOrDefault(m => m.Id == id);

            if (Mentor == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
