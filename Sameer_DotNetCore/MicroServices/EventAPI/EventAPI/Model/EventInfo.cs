using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventAPI.Model
{
    [Table("Events")]
    public class EventInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title of event cannot be empty")]

        [Column(TypeName = "varchar(40)")] //For SQL Data Type
        public string Title { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Start Date is required")]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "End Date is required")]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Organizer name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Minimum 3 and Maximum 50 charaters required")]
        public string Organizer { get; set; }

        [Required(ErrorMessage = "Location of event cannot be empty")]
        public string Location { get; set; }

        [DataType(DataType.Url)]
        [Required(ErrorMessage = "Registration URL of event cannot be empty")]
        [Display(Name = "Registration URL")]
        public string RegistrationURL { get; set; }

        [Required(ErrorMessage = "Speaker Name cannot be empty")]
        public string Speaker { get; set; } // This is the new column we added
    }
        
}
