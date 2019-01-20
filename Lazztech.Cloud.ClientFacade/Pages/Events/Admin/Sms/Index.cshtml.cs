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

        private readonly IRepository _repo;

        public IndexModel(IRepository repository)
        {
            _repo = repository;
        }

        public void OnGet()
        {
            Messages = _repo.All<SmsDto>().ToList();
        }
    }
}