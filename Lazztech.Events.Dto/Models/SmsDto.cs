using System;

namespace Lazztech.Events.Dto.Models
{
    public class SmsDto
    {
        public DateTime DateCreated { get; set; }
        public int Id { get; set; }
        public string ToPhoneNumber { get; set; }
        public string FromPhoneNumber { get; set; }
        public string MessageBody { get; set; }
        public string Sid { get; set; }
        public DateTime? DateTimeWhenProcessed { get; set; }

        public SmsDto()
        {
        }

        public SmsDto(string message, string toNumber, string fromNumber)
        {
            MessageBody = message;
            ToPhoneNumber = toNumber;
            FromPhoneNumber = fromNumber;
        }
    }
}
