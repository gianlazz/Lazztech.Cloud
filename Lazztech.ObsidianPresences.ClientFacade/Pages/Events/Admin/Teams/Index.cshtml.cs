using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lazztech.Cloud.ClientFacade.Pages.Events.Admin.Teams
{
    public class IndexModel : PageModel
    {
        public IList<Team> Team { get; set; }

        private readonly IRepository _repo;

        public IndexModel(IRepository repository)
        {
            _repo = repository;
        }

        public async Task OnGetAsync()
        {
            Team = _repo.All<Team>().ToList();
            //Team = await _context.Team.ToListAsync();
        }
    }
}