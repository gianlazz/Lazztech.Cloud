using System;

namespace Lazztech.Events.Dto.Models
{
    public class Judge
    {
        public Guid Id { get; set; }
        public string Event { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public string ProfessionalTitle { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }

        public Judge()
        {
            Id = Guid.NewGuid();
        }
    }
}