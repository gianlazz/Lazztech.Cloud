using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace Lazztech.Cloud.ClientFacade.Pages.Events.Admin
{
    public class IndexModel : PageModel
    {
        public List<Mentor> Mentors { get; set; }
        public List<Judge> Judges { get; set; }
        public List<Team> Teams { get; set; }
        public List<SmsDto> Messages { get; set; }

        private readonly IDalHelper _db;

        public IndexModel(IDalHelper dal)
        {
            _db = dal;
            Mentors = new List<Mentor>();
            Judges = new List<Judge>();
            Teams = new List<Team>();
            Messages = new List<SmsDto>();
        }

        public void OnGet()
        {
            Mentors.AddRange(_db.All<Mentor>().ToList());
            Judges.AddRange(_db.All<Judge>().ToList());
            Teams.AddRange(_db.All<Team>().ToList());
            Messages = _db.All<SmsDto>().Take(10).ToList();
        }
    }
}