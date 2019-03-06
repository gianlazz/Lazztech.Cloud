using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Marketing.Dal.Dao
{
    public class Campaign
    {
        public int CampaignId { get; set; }
        public string Name { get; set; }
        public DateTime DateStarted { get; set; }
        public string Link { get; set; }

        public Campaign()
        {
            DateStarted = DateTime.Now;
        }
    }
}
