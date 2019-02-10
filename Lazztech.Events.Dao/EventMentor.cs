using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Events.Dao
{
    public class EventMentor
    {
        public int EventId { get; set; }
        public Event Event { get; set; }

        public int MentorId { get; set; }
        public Mentor Mentor { get; set; }
    }
}
