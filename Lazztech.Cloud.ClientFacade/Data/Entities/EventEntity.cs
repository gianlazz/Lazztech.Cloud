using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Cloud.ClientFacade.Data.Entities
{
    public class EventEntity
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public IList<MentorEntity> Mentors { get; set; }
        public IList<JudgeEntity> Judges { get; set; }
        public IList<VolunteerEntity> Volunteers { get; set; }
        public IList<TeamEntity> Teams { get; set; }
        public LocationEntity Location { get; set; }
    }
}
