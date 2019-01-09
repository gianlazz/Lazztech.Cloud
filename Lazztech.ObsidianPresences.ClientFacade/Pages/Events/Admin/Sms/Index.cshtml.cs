using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackathonManager.DTO;
using HackathonManager.RepositoryPattern;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lazztech.Cloud.ClientFacade.Pages.Events.Admin.Sms
{
    public class IndexModel : PageModel
    {
        private IRepository _repo = Startup.DbRepo;

        public List<SmsDto> Messages { get; set; }

        public void OnGet()
        {
            Messages = _repo.All<SmsDto>().ToList();
        }
    }
}