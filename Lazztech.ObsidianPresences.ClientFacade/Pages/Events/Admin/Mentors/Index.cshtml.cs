using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace Lazztech.Cloud.ClientFacade.Pages.Events.Admin.Mentors
{
    public class IndexModel : PageModel
    {
        private readonly IRepository _repo;

        public IndexModel(IRepository repository)
        {
            _repo = repository;
        }

        public IList<Mentor> Mentor { get; set; }

        public void OnGet()
        {
            Mentor = _repo.All<Mentor>().ToList();

            //Mentor = await _context.Mentor.ToListAsync();
        }
    }
}