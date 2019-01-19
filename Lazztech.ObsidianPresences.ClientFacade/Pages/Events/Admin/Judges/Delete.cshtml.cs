using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lazztech.Cloud.ClientFacade.Pages.Events.Admin.Judges
{
    public class DeleteModel : PageModel
    {
        private readonly IRepository _repo;

        public DeleteModel(IRepository repository)
        {
            _repo = repository;
        }

        [BindProperty]
        public Judge Judge { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Judge = await _context.Judge.FirstOrDefaultAsync(m => m.Id == id);
            Judge = _repo.All<Judge>().FirstOrDefault(x => x.Id == id);

            if (Judge == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Judge = await _context.Judge.FindAsync(id);
            Judge = _repo.All<Judge>().FirstOrDefault(x => x.Id == id);

            if (Judge != null)
            {
                //_context.Judge.Remove(Judge);
                //await _context.SaveChangesAsync();
                _repo.Delete<Judge>(Judge);
            }

            return RedirectToPage("./Index");
        }
    }
}