using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lazztech.Cloud.ClientFacade.Pages.Events.Admin.Teams
{
    public class DetailsModel : PageModel
    {
        //private readonly Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext _context;

        //public DetailsModel(Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext context)
        //{
        //    _context = context;
        //}
        private IRepository _repo = Startup.DbRepo;

        public Team Team { get; set; }

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
    }
}