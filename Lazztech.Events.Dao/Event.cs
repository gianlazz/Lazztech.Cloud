using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Events.Dao
{
    public class Event
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        public DateTime? DateOfEvent { get; set; }

        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }

        public List<EventMentor> EventMentors { get; set; }

        public Event()
        {
            EventMentors = new List<EventMentor>();
        }
    }
}
