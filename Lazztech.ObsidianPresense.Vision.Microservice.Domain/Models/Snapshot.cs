using System;
using System.Collections.Generic;

namespace Lazztech.ObsidianPresense.Vision.Microservice.Domain.Models
{
    public class Snapshot
    {
        public Snapshot()
        {
        }

        public string Location
        {
            get;
            set;
        }

        public List<Person> People
        {
            get;
            set;
        }
    }
}
