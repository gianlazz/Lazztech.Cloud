using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lazztech.Events.Dal.Dao
{
    public class Sms
    {
        [Display(Name = "Sms")]
        public int SmsId { get; set; }
        public DateTime DateCreated { get; set; }
        public string ToPhoneNumber { get; set; }
        public string FromPhoneNumber { get; set; }
        public string MessageBody { get; set; }
        public string Sid { get; set; }
        public DateTime? DateTimeWhenProcessed { get; set; }

        public Sms()
        {
            DateCreated = DateTime.Now;
        }
    }
}
