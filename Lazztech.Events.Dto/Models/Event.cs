using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Events.Dto.Models
{
    class Event
    {
        public Guid Id { get; set; }
        public string Location { get; set; }
        public List<string> VenueLocations { get; set; }
        public List<Mentor> Mentors { get; set; }
        public List<Judge> Judges { get; set; }
        public List<Team> Teams { get; set; }

        public Event()
        {
            Id = Guid.NewGuid();
        }
    }
}
