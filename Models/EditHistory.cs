using System.ComponentModel.DataAnnotations;

namespace ContactsApp.Models
{
    public class EditHistory
    {
        public int Id { get; set; }
        [Required]
        public DateTime ModifiedDate { get; set; }
        [Required]
        public int ContactId { get; set; }
    }
}
