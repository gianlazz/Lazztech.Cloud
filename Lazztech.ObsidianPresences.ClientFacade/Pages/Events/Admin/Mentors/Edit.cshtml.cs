using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lazztech.Cloud.ClientFacade.Pages.Events.Admin.Mentors
{
    public class EditModel : PageModel
    {
        //private readonly Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext _context;

        //public EditModel(Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext context)
        //{
        //    _context = context;
        //}
        private IRepository _repo = Startup.DbRepo;

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //_context.Attach(Mentor).State = EntityState.Modified;

            try
            {
                _repo.Delete<Mentor>(x => x.Id == Mentor.Id);
                _repo.Add<Mentor>(Mentor);
                //await _context.SaveChangesAsync();
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
            return _repo.All<Mentor>().Any(x => x.Id == id);
            //return _context.Mentor.Any(e => e.Id == id);
        }
    }
}