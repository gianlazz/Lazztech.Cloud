using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lazztech.Cloud.ClientFacade.Pages.Events.Admin.Mentors
{
    public class DetailsModel : PageModel
    {
        public Mentor Mentor { get; set; }

        private readonly IDalHelper _db;

        public DetailsModel(IDalHelper dal)
        {
            _db = dal;
        }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Mentor = await _context.Mentor.FirstOrDefaultAsync(m => m.Id == id);
            Mentor = _db.All<Mentor>().FirstOrDefault(m => m.Id == id);

            if (Mentor == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}