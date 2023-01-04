using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ContactsApp.Models
{
    public class Contact
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 3)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 3)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

    }
}
