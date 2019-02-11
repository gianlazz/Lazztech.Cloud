using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Events.Dal.Dao
{
    public class MentorRequest
    {
        public int MentorRequestId { get; set; }
        public DateTime DateTimeOfRequest { get; set; }

        public int MentorId { get; set; }
        public Mentor Mentor { get; set; }

        public string UniqueRequesteeId { get; set; }
        public bool RequestAccepted { get; set; }
        public DateTime? DateTimeWhenProcessed { get; set; }
        public Sms OutboundSms { get; set; }
        public Sms InboundSms { get; set; }
        public TimeSpan MentoringDuration { get; set; }
        public TimeSpan RequestTimeout { get; set; }
        public bool TimedOut { get; set; }

        public MentorRequest()
        {
            DateTimeOfRequest = DateTime.Now;

            MentoringDuration = new TimeSpan(hours: 0, minutes: 12, seconds: 0);
            RequestTimeout = new TimeSpan(hours: 0, minutes: 5, seconds: 0);
        }
    }
}
