using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lazztech.Cloud.ClientFacade.Data;
using Lazztech.Events.Dal.Dao;

namespace Lazztech.Cloud.ClientFacade.Pages.Admin.EventManagement.Sms
{
    public class IndexModel : PageModel
    {
        private readonly Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext _context;

        public IndexModel(Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Lazztech.Events.Dal.Dao.Sms> Sms { get;set; }

        public async Task OnGetAsync()
        {
            Sms = await _context.SmsMessages.ToListAsync();
        }
    }
}
