using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lazztech.Cloud.ClientFacade.Pages.Events.Admin.Judges
{
    public class DetailsModel : PageModel
    {
        public Judge Judge { get; set; }

        private readonly IRepository _repo;

        public DetailsModel(IRepository repository)
        {
            _repo = repository;
        }

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