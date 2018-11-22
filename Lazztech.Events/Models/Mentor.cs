using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonManager.DTO
{
    public class Mentor
    {
        //public System.Guid GuidId { get; set { this.GuidId = Guid.NewGuid(); } }
        //public Guid GuidId { get; set; } = Guid.NewGuid();
        public System.Guid GuidId { get; set; }
        public string Event { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public int Age { get; set; }
        public string Description { get; set; }
        public string ProfessionalTitle { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsPresent { get; set; }
        public string MentorType { get; set; }

        public Mentor()
        {
            GuidId = Guid.NewGuid();
        }
    }
}
