using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace Lazztech.Cloud.ClientFacade.Pages.Events.Admin.Sms
{
    public class IndexModel : PageModel
    {
        public List<SmsDto> Messages { get; set; }

        private readonly IDalHelper _db;

        public IndexModel(IDalHelper dal)
        {
            _db = dal;
        }

        public void OnGet()
        {
            Messages = _db.All<SmsDto>().ToList();
        }
    }
}