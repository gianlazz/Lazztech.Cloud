using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Cloud.ClientFacade.Data.Entities
{
    public class Person
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public int Description { get; set; }
    }
}
