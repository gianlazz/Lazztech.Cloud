using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lazztech.Cloud.ClientFacade.Data;
using Lazztech.Marketing.Dal.Dao;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lazztech.Cloud.ClientFacade.Pages.Marketing.FocusGroupPage
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public FocusGroup FocusGroup { get; set; }

        public IndexModel(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public void OnGet(int? Id)
        {
            if (Id != null)
                FocusGroup = _context.FocusGroups.FirstOrDefault(x => x.FocusGroupId == Id);
        }
    }
}