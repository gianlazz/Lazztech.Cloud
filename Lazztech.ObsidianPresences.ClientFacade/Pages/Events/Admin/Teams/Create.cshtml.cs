using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Lazztech.Cloud.ClientFacade.Pages.Events.Admin.Teams
{
    public class CreateModel : PageModel
    {
        private readonly IRepository _repo;

        public CreateModel(IRepository repository)
        {
            _repo = repository;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Team Team { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //_context.Team.Add(Team);
            //await _context.SaveChangesAsync();
            _repo.Add<Team>(Team);

            return RedirectToPage("./Index");
        }
    }
}