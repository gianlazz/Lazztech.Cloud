using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazztech.Events.Dto.Models
{
    public class Mentor
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string FullName { get { return $"{FirstName} {LastName}"; } }
        public string Image { get; set; }
        public string Description { get; set; }
        public string ProfessionalTitle { get; set; }

        [Required(ErrorMessage = "You must provide a cell phone number")]
        [Display(Name = "Cell Phone")]
        [DataType(DataType.PhoneNumber)]

        [MaxLength(10)]
        [MinLength(10)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Please enter phone number: 5555555555")]
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsPresent { get; set; }

        public Mentor()
        {
            IsAvailable = true;
            IsPresent = true;
        }
    }
}
