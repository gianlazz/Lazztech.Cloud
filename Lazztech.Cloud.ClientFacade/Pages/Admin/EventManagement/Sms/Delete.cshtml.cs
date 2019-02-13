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
    public class DeleteModel : PageModel
    {
        private readonly Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext _context;

        public DeleteModel(Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Lazztech.Events.Dal.Dao.Sms Sms { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sms = await _context.SmsMessages.FirstOrDefaultAsync(m => m.SmsId == id);

            if (Sms == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sms = await _context.SmsMessages.FindAsync(id);

            if (Sms != null)
            {
                _context.SmsMessages.Remove(Sms);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
