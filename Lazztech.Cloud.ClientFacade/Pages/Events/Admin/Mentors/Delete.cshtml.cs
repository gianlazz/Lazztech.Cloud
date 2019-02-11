using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lazztech.Cloud.ClientFacade.Pages.Events.Admin.Mentors
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public Mentor Mentor { get; set; }

        private readonly IDalHelper _db;

        public DeleteModel(IDalHelper dal)
        {
            _db = dal;
        }

        public async Task<IActionResult> OnGet(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Mentor = await _context.Mentor.FirstOrDefaultAsync(m => m.Id == id);
            Mentor = _db.All<Mentor>().FirstOrDefault(x => x.Id == id);

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
            Mentor = _db.All<Mentor>().FirstOrDefault(x => x.Id == id);

            if (Mentor != null)
            {
                //_context.Mentor.Remove(Mentor);
                //await _context.SaveChangesAsync();
                _db.Delete<Mentor>(Mentor);
            }

            return RedirectToPage("./Index");
        }
    }
}