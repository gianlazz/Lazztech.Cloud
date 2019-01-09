using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

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