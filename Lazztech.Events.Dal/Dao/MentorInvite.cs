﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lazztech.Events.Dal.Dao
{
    public class MentorInvite
    {
        [Display(Name = "Mentor Invite")]
        public int MentorInviteId { get; set; }

        public DateTime DateTimeWhenCreated { get; set; }

        [Display(Name = "Event")]
        public int EventId { get; set; }
        public Event Event { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        [Display(Name = "Mentor")]
        public int? MentorId { get; set; }
        public Mentor Mentor { get; set; }

        public DateTime? DateTimeWhenViewed { get; set; }
        public bool Accepted { get; set; }
        public string InviteLink { get; set; }

        public MentorInvite()
        {
            DateTimeWhenCreated = DateTime.Now;
        }
    }
}
