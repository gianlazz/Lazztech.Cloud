using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lazztech.Cloud.ClientFacade.Data;
using Lazztech.Marketing.Dal.Dao;

namespace Lazztech.Cloud.ClientFacade.Pages.Marketing.FocusGroups
{
    public class DetailsModel : PageModel
    {
        private readonly Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext _context;

        public DetailsModel(Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public FocusGroup FocusGroup { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FocusGroup = await _context.FocusGroups.FirstOrDefaultAsync(m => m.FocusGroupId == id);

            if (FocusGroup == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
