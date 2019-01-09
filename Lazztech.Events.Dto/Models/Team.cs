using Lazztech.Events.Dto.Enums;
using System;

namespace Lazztech.Events.Dto.Models
{
    public class Team
    {
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public RoomNameEnum Location { get; set; }
        public int PinNumber { get; set; }

        public Team()
        {
            Id = Guid.NewGuid();
        }
    }
}