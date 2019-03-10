﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lazztech.Cloud.ClientFacade.Data;
using Lazztech.Marketing.Dal.Dao;

namespace Lazztech.Cloud.ClientFacade.Pages.Marketing.Admin.InstagramNodes
{
    public class DeleteModel : PageModel
    {
        private readonly Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext _context;

        public DeleteModel(Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InstagramNode InstagramNode { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InstagramNode = await _context.InstagramNodes
                .Include(i => i.FocusGroup).FirstOrDefaultAsync(m => m.InstagramNodeId == id);

            if (InstagramNode == null)
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

            InstagramNode = await _context.InstagramNodes.FindAsync(id);

            if (InstagramNode != null)
            {
                _context.InstagramNodes.Remove(InstagramNode);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
