using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Marketing.Dal.Dao
{
    public class InstagramPost
    {
        public int InstagramPostId { get; set; }
        public bool Posted { get; set; }
        public DateTime? DateTimeForPost { get; set; }
        public string LinkToContent { get; set; }
        public string Caption { get; set; }
        public string PostUrl { get; set; }

        public int? InstagramNodeId { get; set; }
        public InstagramNode InstagramNode { get; set; }
    }
}
