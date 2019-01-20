using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Cloud.ClientFacade.Data.Entities
{
    public class MentorEntity
    {
        public int Id { get; set; }
        public PersonEntity Person { get; set; }
        public string PhoneNumber { get; set; }
    }
}
