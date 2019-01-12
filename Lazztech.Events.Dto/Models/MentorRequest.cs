using System;

namespace Lazztech.Events.Dto.Models
{
    public class MentorRequest
    {
        public readonly DateTime DateTimeOfRequest;
        public readonly Guid GuidId;
        public Mentor Mentor { get; set; }
        public string UniqueRequesteeId { get; set; }
        public bool RequestAccepted { get; set; }
        public DateTime? DateTimeWhenProcessed { get; set; }
        public SmsDto OutboundSms { get; set; }
        public SmsDto InboundSms { get; set; }
        public TimeSpan Timeout { get; set; }

        public MentorRequest()
        {
            DateTimeOfRequest = DateTime.Now;
            GuidId = Guid.NewGuid();

            Timeout = new TimeSpan(hours: 0, minutes: 15, seconds: 0);
        }
    }
}