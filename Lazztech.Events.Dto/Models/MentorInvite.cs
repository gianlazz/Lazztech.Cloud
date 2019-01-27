using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Events.Dto.Models
{
    public class MentorInvite
    {
        public Guid Id { get; set; }
        public DateTime DateTimeWhenCreated { get; set; }
        public Mentor Mentor { get; set; }
        public DateTime? DateTimeWhenViewed { get; set; }
        public bool Accepted { get; set; }
        public string InviteLink { get; set; }

        public MentorInvite()
        {
            DateTimeWhenCreated = DateTime.Now;
            Id = Guid.NewGuid();
        }
    }
}
