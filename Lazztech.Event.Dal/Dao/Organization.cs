using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Events.Dal.Dao
{
    public class Organization
    {
        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public List<Event> Events { get; set; }

        public Organization()
        {
            Events = new List<Event>();
        }
    }
}
