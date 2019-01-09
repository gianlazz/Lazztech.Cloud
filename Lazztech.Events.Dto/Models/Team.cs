using HackathonManager.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonManager.PocoModels
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
