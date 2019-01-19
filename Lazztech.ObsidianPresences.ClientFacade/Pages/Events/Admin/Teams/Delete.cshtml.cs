using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lazztech.Cloud.ClientFacade.Pages.Events.Admin.Teams
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public Team Team { get; set; }

        private readonly IRepository _repo;

        public DeleteModel(IRepository repository)
        {
            _repo = repository;
        }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Team = await _context.Team.FirstOrDefaultAsync(m => m.Id == id);
            Team = _repo.All<Team>().FirstOrDefault(m => m.Id == id);

            if (Team == null)
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

            //Team = await _context.Team.FindAsync(id);
            Team = _repo.All<Team>().FirstOrDefault(x => x.Id == id);

            if (Team != null)
            {
                //_context.Team.Remove(Team);
                //await _context.SaveChangesAsync();
                _repo.Delete<Team>(Team);
            }

            return RedirectToPage("./Index");
        }
    }
}