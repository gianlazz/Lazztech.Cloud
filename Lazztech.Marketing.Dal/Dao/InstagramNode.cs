using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Marketing.Dal.Dao
{
    public class InstagramNode
    {
        public int InstagramNodeId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateStarted { get; set; }
        public string Link { get; set; }

        public int FocusGroupId { get; set; }
        public FocusGroup FocusGroup { get; set; }

        public List<InstagramPost> InstagramPosts { get; set; }

        public InstagramNode()
        {
            DateStarted = DateTime.Now;
        }
    }
}
