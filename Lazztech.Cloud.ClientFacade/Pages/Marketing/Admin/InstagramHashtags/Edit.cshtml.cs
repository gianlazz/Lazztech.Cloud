using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lazztech.Cloud.ClientFacade.Data;
using Lazztech.Marketing.Dal.Dao;

namespace Lazztech.Cloud.ClientFacade.Pages.Marketing.Admin.InstagramHashtags
{
    public class EditModel : PageModel
    {
        private readonly Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext _context;

        public EditModel(Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InstagramHashtag InstagramHashtag { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InstagramHashtag = await _context.InstagramHashtags
                .Include(i => i.FocusGroup).FirstOrDefaultAsync(m => m.InstagramHashtagId == id);

            if (InstagramHashtag == null)
            {
                return NotFound();
            }
           ViewData["FocusGroupId"] = new SelectList(_context.FocusGroups, "FocusGroupId", "FocusGroupId");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(InstagramHashtag).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstagramHashtagExists(InstagramHashtag.InstagramHashtagId))
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

        private bool InstagramHashtagExists(int id)
        {
            return _context.InstagramHashtags.Any(e => e.InstagramHashtagId == id);
        }
    }
}
