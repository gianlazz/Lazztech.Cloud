using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Dal.DBModels
{
    public class Volunteer
    {
        public int Id { get; set; }
        public Person Person { get; set; }
    }
}
