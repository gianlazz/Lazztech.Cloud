﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Cloud.ClientFacade.Data.Entities
{
    public class VolunteerEntity
    {
        public int Id { get; set; }
        public PersonEntity Person { get; set; }
    }
}