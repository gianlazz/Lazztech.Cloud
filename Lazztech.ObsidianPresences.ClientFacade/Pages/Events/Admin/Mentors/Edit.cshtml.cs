using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackathonManager.DTO;
using HackathonManager.RepositoryPattern;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lazztech.ObsidianPresences.ClientFacade.Pages.Events.Admin.Mentors
{
    public class EditModel : PageModel
    {
        private IRepository _repo = Startup.DbRepo;
        public Mentor Mentor { get; set; }

        public void OnGet(Guid id)
        {
            if (id == Guid.Empty)
                Mentor = _repo.All<Mentor>().Where(x => x.GuidId == null).ToList().First();
            else
                Mentor = _repo.All<Mentor>().Where(x => x.GuidId == id).ToList().First();
        }

        //public ActionResult Edit(Guid id)
        //{
        //    Mentor mentor;
        //    if (id == Guid.Empty)
        //        mentor = _repo.All<Mentor>().Where(x => x.GuidId == null).ToList().First();
        //    else
        //        mentor = _repo.All<Mentor>().Where(x => x.GuidId == id).ToList().First();

        //    return View(mentor);
        //}
    }
}