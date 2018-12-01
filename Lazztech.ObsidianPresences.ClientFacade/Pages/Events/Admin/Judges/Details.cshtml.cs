using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HackathonManager;
using Lazztech.ObsidianPresences.ClientFacade.Data;
using HackathonManager.RepositoryPattern;

namespace Lazztech.ObsidianPresences.ClientFacade.Pages.Events.Admin.Judges
{
    public class DetailsModel : PageModel
    {
        //private readonly Lazztech.ObsidianPresences.ClientFacade.Data.ApplicationDbContext _context;

        //public DetailsModel(Lazztech.ObsidianPresences.ClientFacade.Data.ApplicationDbContext context)
        //{
        //    _context = context;
        //}
        private IRepository _repo = Startup.DbRepo;

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
    }
}
