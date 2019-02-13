using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lazztech.Cloud.ClientFacade.Data;
using Lazztech.Events.Dal.Dao;

namespace Lazztech.Cloud.ClientFacade.Pages.Admin.EventManagement.Sms
{
    public class EditModel : PageModel
    {
        private readonly Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext _context;

        public EditModel(Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext context)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Sms).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SmsExists(Sms.SmsId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool SmsExists(int id)
        {
            return _context.SmsMessages.Any(e => e.SmsId == id);
        }
    }
}
