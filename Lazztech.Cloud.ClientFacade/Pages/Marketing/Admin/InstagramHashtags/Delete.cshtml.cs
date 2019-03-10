using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lazztech.Cloud.ClientFacade.Data;
using Lazztech.Marketing.Dal.Dao;

namespace Lazztech.Cloud.ClientFacade.Pages.Marketing.Admin.InstagramHashtags
{
    public class DeleteModel : PageModel
    {
        private readonly Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext _context;

        public DeleteModel(Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext context)
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InstagramHashtag = await _context.InstagramHashtags.FindAsync(id);

            if (InstagramHashtag != null)
            {
                _context.InstagramHashtags.Remove(InstagramHashtag);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
