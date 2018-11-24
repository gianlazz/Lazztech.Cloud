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
    public class IndexModel : PageModel
    {
        private IRepository _repo = Startup.DbRepo;
        public List<Mentor> Mentors { get; set; }

        public IndexModel()
        {

        }

        public void OnGet()
        {
            Mentors = _repo.All<Mentor>().ToList();

            //var downloader = new HackathonManager.SrndResourcesManager.SrndMentorCsvDownloader();
            //var parser = new HackathonManager.SrndMentorCsvParser();
            //var newMentors = parser.Parse(downloader.GetCsv());
            //_srndMentors = newMentors.Where(x => x.MentorType.ToLower().Trim() == "mentor").ToList();

            //var areNewMentors = !_srndMentors.Except(mentors).Any();

            //ViewBag.MentorsToPull = _srndMentors.Count();
        }
    }
}