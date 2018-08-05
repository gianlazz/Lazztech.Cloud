﻿using System;
using System.Collections.Generic;

namespace Lazztech.ObsidianPresenses.Vision.Microservice.Domain.Models
{
    public class Person
    {
        public Person()
        {
            FaceBoundingBox = new FaceBoundingBox();
            Mood = new Dictionary<string, Likelyhood>();
        }
        public string Name { get; set; }
        public FaceBoundingBox FaceBoundingBox { get; set; }
        public Dictionary<string, Likelyhood> Mood { get; set; }
        public string HeartRate { get; set; }
    }
}
