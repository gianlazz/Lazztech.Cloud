using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Marketing.Dal.Dao
{
    public class InstagramHashtag
    {
        public int InstagramHashtagId { get; set; }
        public string Name { get; set; }

        public int FocusGroupId { get; set; }
        public FocusGroup FocusGroup { get; set; }

        public InstagramHashtag()
        {
        }
    }
}
