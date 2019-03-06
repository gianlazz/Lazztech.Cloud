using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Marketing.Dal.Dao
{
    public class FocusGroup
    {
        public int FocusGroupId { get; set; }
        public string Name { get; set; }
        public DateTime DateStarted { get; set; }
        public List<InstagramNode> InstagramNodes { get; set; }

        public FocusGroup()
        {
            InstagramNodes = new List<InstagramNode>();
        }
    }
}
