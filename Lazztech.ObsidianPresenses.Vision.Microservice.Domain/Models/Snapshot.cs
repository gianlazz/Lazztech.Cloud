using System;
using System.Collections.Generic;

namespace Lazztech.ObsidianPresenses.Vision.Microservice.Domain.Models
{
    public class Snapshot
    {
        public Snapshot()
        {
        }

        public Guid GuidId
        {
            get;
            set;
        }

        public String ImagePathName
        {
            get;
            set;
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
