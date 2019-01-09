using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonManager.DTO
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
        public DateTime? DateTimeWhenProcessed { get; internal set; }

        public SmsDto()
        {
            GuidId = Guid.NewGuid();
        }
    }
}
