using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lazztech.Cloud.ClientFacade.Pages.Events.Admin.Judges
{
    public class IndexModel : PageModel
    {
        private readonly IRepository _repo;

        public IList<Judge> Judge { get; set; }

        public IndexModel(IRepository repository)
        {
            _repo = repository;
        }

        public async Task OnGetAsync()
        {
            Judge = _repo.All<Judge>().ToList();
            //Judge = await _context.Judge.ToListAsync();
        }
    }
}