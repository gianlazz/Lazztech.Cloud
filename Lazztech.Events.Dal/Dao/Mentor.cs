using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lazztech.Events.Dal.Dao
{
    public class Mentor
    {
        [Display(Name = "Mentor")]
        public int MentorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return $"{FirstName} {LastName}"; } }
        public string Image { get; set; }
        public string Skills { get; set; }
        public string ProfessionalTitle { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsPresent { get; set; }

        [Display(Name = "Event")]
        public int? EventId { get; set; }
        public Event Event { get; set; }

        [Display(Name = "Mentor Invite")]
        public int? MentorInviteId { get; set; }
        public MentorInvite MentorInvite { get; set; }

        public Mentor()
        {
            IsAvailable = true;
            IsPresent = true;
        }
    }
}
