using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.ObsidianPresences.ClientFacade.Data.Entities
{
    public class Mentor
    {
        public int Id { get; set; }
        public Person Person { get; set; }
        public string PhoneNumber { get; set; }
    }
}
