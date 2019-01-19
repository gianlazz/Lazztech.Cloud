using System;

namespace Lazztech.Events.Dto.Models
{
    public class SmsDto
    {
        public DateTime DateCreated { get; set; }
        public Guid GuidId { get; set; }
        public string ToPhoneNumber { get; set; }
        public string FromPhoneNumber { get; set; }
        public string MessageBody { get; set; }
        public string Sid { get; set; }
        public DateTime? DateTimeWhenProcessed { get; set; }

        public SmsDto()
        {
            GuidId = Guid.NewGuid();
        }

        public SmsDto(string message, string toNumber, string fromNumber)
        {
            GuidId = Guid.NewGuid();
            MessageBody = message;
            ToPhoneNumber = toNumber;
            FromPhoneNumber = fromNumber;
        }
    }
}
