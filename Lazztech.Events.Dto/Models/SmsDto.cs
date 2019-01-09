using System;

namespace Lazztech.Events.Dto.Models
{
    public class SmsDto
    {
        public DateTime DateCreated { get; set; }
        public readonly Guid GuidId;
        public string ToPhoneNumber { get; set; }
        public string FromPhoneNumber { get; set; }
        public string MessageBody { get; set; }
        /// <summary>
        /// A string that uniquely identifies this message
        /// </summary>
        public string Sid { get; set; }
        public DateTime? DateTimeWhenProcessed { get; set; }

        public SmsDto()
        {
            GuidId = Guid.NewGuid();
        }
    }
}
