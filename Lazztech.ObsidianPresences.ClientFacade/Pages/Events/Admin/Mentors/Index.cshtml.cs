using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace Lazztech.Cloud.ClientFacade.Pages.Events.Admin.Mentors
{
    public class IndexModel : PageModel
    {
        private readonly Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext _context;
        private IRepository _repo = Startup.DbRepo;

        public IndexModel(Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Mentor> Mentor { get; set; }

        public void OnGet()
        {
            Mentor = _repo.All<Mentor>().ToList();

            //Mentor = await _context.Mentor.ToListAsync();
        }
    }
}