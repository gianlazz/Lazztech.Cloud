using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lazztech.Events.Dal.Dao
{
    public class Event
    {
        [Display(Name = "Event")]
        public int EventId { get; set; }
        public string Name { get; set; }
        public DateTime? DateOfEvent { get; set; }

        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }

        public List<Mentor> Mentors { get; set; }

        public Event()
        {
            Mentors = new List<Mentor>();
        }
    }
}
