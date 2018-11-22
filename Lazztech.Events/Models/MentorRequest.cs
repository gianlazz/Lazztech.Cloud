using HackathonManager.DTO;
using HackathonManager.PocoModels;
using System;

namespace HackathonManager.Models
{
    public class MentorRequest
    {
        public readonly DateTime DateTimeOfRequest;
        public readonly Guid GuidId;
        public Mentor Mentor { get; set; }
        public Team Team { get; set; }
        public bool RequestAccepted { get; internal set; }
        public DateTime? DateTimeWhenProcessed { get; set; }

        public bool? OverTaskTimeLimit { get; set; }
        public DateTime? TaskTimeLimit { get; set; }
        public bool? RequestTimedOut { get; set; }
        public DateTime? RequestTimeLimit { get; set; }

        public SmsDto OutboundSms { get; set; }
        public SmsDto InboundSms { get; set; }

        public MentorRequest()
        {
            DateTimeOfRequest = DateTime.Now;
            GuidId = Guid.NewGuid();
        }
    }
}