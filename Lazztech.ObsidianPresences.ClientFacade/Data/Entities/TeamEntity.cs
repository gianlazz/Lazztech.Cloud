using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Cloud.ClientFacade.Data.Entities
{
    public class TeamEntity
    {
        public int Id { get; set; }
        public VenueRoomEntity VenueRoom { get; set; }
        public string Name { get; internal set; }
    }
}
