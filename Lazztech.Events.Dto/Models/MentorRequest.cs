using System;

namespace Lazztech.Events.Dto.Models
{
    public class MentorRequest
    {
        public Guid Id { get; set; }
        public readonly DateTime DateTimeOfRequest;
        public Mentor Mentor { get; set; }
        public string UniqueRequesteeId { get; set; }
        public bool RequestAccepted { get; set; }
        public DateTime? DateTimeWhenProcessed { get; set; }
        public SmsDto OutboundSms { get; set; }
        public SmsDto InboundSms { get; set; }
        public TimeSpan MentoringDuration { get; set; }
        public TimeSpan RequestTimeout { get; set; }
        public bool TimedOut { get; set; }

        public MentorRequest()
        {
            DateTimeOfRequest = DateTime.Now;
            Id = Guid.NewGuid();

            MentoringDuration = new TimeSpan(hours: 0, minutes: 12, seconds: 0);
            RequestTimeout = new TimeSpan(hours: 0, minutes: 5, seconds: 0);
        }
    }
}