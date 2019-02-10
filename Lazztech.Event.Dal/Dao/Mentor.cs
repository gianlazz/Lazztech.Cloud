using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Events.Dal.Dao
{
    public class Mentor
    {
        public int MentorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return $"{FirstName} {LastName}"; } }
        public string Image { get; set; }
        public string Description { get; set; }
        public string ProfessionalTitle { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsPresent { get; set; }

        public List<EventMentor> EventMentors { get; set; }

        public Mentor()
        {
            IsAvailable = true;
            IsPresent = true;

            EventMentors = new List<EventMentor>();
        }
    }
}
