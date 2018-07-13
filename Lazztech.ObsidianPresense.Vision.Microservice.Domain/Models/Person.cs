using System;
namespace Lazztech.ObsidianPresense.Vision.Microservice.Domain.Models
{
    public class Person
    {
        public Person()
        {
        }

        public string Name { get; set; }

        public FaceBox FaceBoxes
        {
            get;
            set;
        }

        public string Mood
        {
            get;
            set;
        }

        public string HeartRate
        {
            get;
            set;
        }

    }
}
