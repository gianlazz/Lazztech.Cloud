using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HackathonManager.DTO;
using Lazztech.Cloud.ClientFacade.Data;
using HackathonManager.RepositoryPattern;

namespace Lazztech.Cloud.ClientFacade.Pages.Events.Admin.Mentors
{
    public class DeleteModel : PageModel
    {
        //private readonly Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext _context;

        //public DeleteModel(Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        private IRepository _repo = Startup.DbRepo;

        [BindProperty]
        public Mentor Mentor { get; set; }

        public async Task<IActionResult> OnGet(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Mentor = await _context.Mentor.FirstOrDefaultAsync(m => m.Id == id);
            Mentor = _repo.All<Mentor>().FirstOrDefault(x => x.Id == id);

            if (Mentor == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPost(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Mentor = await _context.Mentor.FindAsync(id);
            Mentor = _repo.All<Mentor>().FirstOrDefault(x => x.Id == id);

            if (Mentor != null)
            {
                //_context.Mentor.Remove(Mentor);
                //await _context.SaveChangesAsync();
                _repo.Delete<Mentor>(Mentor);
            }

            return RedirectToPage("./Index");
        }
    }
}
