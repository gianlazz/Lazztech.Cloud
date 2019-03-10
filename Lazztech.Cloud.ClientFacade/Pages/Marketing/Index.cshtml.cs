using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lazztech.Cloud.ClientFacade.Data;
using Lazztech.Marketing.Dal.Dao;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Lazztech.Cloud.ClientFacade.Pages.Marketing
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        public int CampaignCount { get; set; }
        public int FocusGroupCount { get; set; }
        public int InstagramNodeCount { get; set; }
        public int InstagramHashtagCount { get; private set; }
        public int InstagramPostCount { get; set; }
        public int SelectedInstaContentCount { get; set; }

        public List<FocusGroup> FocusGroups { get; set; }

        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task OnGetAsync()
        {
            CampaignCount = await _context.Campaigns.CountAsync();
            FocusGroupCount = await _context.FocusGroups.CountAsync();
            InstagramNodeCount = await _context.InstagramNodes.CountAsync();
            InstagramHashtagCount = await _context.InstagramHashtags.CountAsync();
            InstagramPostCount = await _context.InstagramPosts.CountAsync();
            SelectedInstaContentCount = await _context.SelectedInstaContents.CountAsync();

            FocusGroups = await _context.FocusGroups.ToListAsync();
        }
    }
}