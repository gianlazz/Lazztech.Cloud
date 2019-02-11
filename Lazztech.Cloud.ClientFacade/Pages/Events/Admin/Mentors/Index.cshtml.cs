using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace Lazztech.Cloud.ClientFacade.Pages.Events.Admin.Mentors
{
    public class IndexModel : PageModel
    {
        private readonly IDalHelper _db;

        public IndexModel(IDalHelper dal)
        {
            _db = dal;
        }

        public IList<Mentor> Mentor { get; set; }

        public void OnGet()
        {
            Mentor = _db.All<Mentor>().ToList();

            //Mentor = await _context.Mentor.ToListAsync();
        }
    }
}