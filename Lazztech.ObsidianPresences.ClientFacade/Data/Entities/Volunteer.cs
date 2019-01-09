using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Cloud.ClientFacade.Data.Entities
{
    public class Volunteer
    {
        public int Id { get; set; }
        public Person Person { get; set; }
    }
}
