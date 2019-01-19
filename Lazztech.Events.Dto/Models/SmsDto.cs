using System;

namespace Lazztech.Events.Dto.Models
{
    public class SmsDto
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string ToPhoneNumber { get; set; }
        public string FromPhoneNumber { get; set; }
        public string MessageBody { get; set; }
        public string Sid { get; set; }
        public DateTime? DateTimeWhenProcessed { get; set; }

        public SmsDto()
        {
            Id = Guid.NewGuid();
        }

        public SmsDto(string message, string toNumber, string fromNumber)
        {
            Id = Guid.NewGuid();
            MessageBody = message;
            ToPhoneNumber = toNumber;
            FromPhoneNumber = fromNumber;
        }
    }
}
