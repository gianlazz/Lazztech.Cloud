﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Lazztech.Cloud.ClientFacade.Data;
using Lazztech.Marketing.Dal.Dao;

namespace Lazztech.Cloud.ClientFacade.Pages.Marketing.Admin.InstagramNodes
{
    public class CreateModel : PageModel
    {
        private readonly Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext _context;

        public CreateModel(Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["FocusGroupId"] = new SelectList(_context.FocusGroups, "FocusGroupId", "FocusGroupId");
            return Page();
        }

        [BindProperty]
        public InstagramNode InstagramNode { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.InstagramNodes.Add(InstagramNode);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}