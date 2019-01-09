using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HackathonManager.DTO;
using Lazztech.Cloud.ClientFacade.Data;
using HackathonManager.RepositoryPattern;

namespace Lazztech.Cloud.ClientFacade.Pages.Events.Admin.Mentors
{
    public class CreateModel : PageModel
    {
        private readonly Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext _context;
        private IRepository _repo = Startup.DbRepo;

        public CreateModel(Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Mentor Mentor { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //_context.Mentor.Add(Mentor);
            //await _context.SaveChangesAsync();
            _repo.Add<Mentor>(Mentor);

            return RedirectToPage("./Index");
        }
    }
}