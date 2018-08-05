using System;
using System.Collections.Generic;

namespace Lazztech.ObsidianPresenses.Vision.Microservice.Domain.Models
{
    public class Snapshot
    {
        public Snapshot()
        {
            People = new List<Person>();
            GuidId = Guid.NewGuid();
        }

        public Guid GuidId { get; private set; }

        public string ImageDir { get; set; }

        public string ImageName { get; set; }

        public string DateTimeWhenCaptured { get; set; }

        public string Location { get; set; }

        public SnapshotStatus Status { get; set; } 

        public List<Person> People { get; set; }

        public enum SnapshotStatus {
            known,
            unknown,
            known_unknown,
            unknown_person,
            no_persons_found
        }
    }
}
