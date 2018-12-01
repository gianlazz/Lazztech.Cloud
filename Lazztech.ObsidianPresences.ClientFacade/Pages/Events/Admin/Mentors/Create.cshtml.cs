using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackathonManager.DTO;
using HackathonManager.RepositoryPattern;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lazztech.ObsidianPresences.ClientFacade.Pages.Events.Admin.Mentors
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Mentor Mentor { get; set; }

        private IRepository _repo = Startup.DbRepo;

        public void OnGet()
        {
            Mentor = new Mentor();
        }

        [HttpPost]
        public void Edit(Mentor mentor)
        {
            try
            {
                // TODO: Add update logic here
                _repo.Delete<Mentor>(x => x.GuidId == mentor.GuidId);
                _repo.Add<Mentor>(mentor);

                Redirect("Index");
            }
            catch
            {
            }
        }
    }
}