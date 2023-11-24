using System.ComponentModel.DataAnnotations;

namespace skolesystem.DTOs
{
    public class AbsenceCreateDto
    {
        [Required(ErrorMessage = "User ID is required")]
        public int user_id { get; set; }

        [Required(ErrorMessage = "Teacher ID is required")]
        public int teacher_id { get; set; }

        [Required(ErrorMessage = "Class ID is required")]
        public int class_id { get; set; }

        [Required(ErrorMessage = "Absence date is required")]
        public DateTime absence_date { get; set; }

        [StringLength(255, ErrorMessage = "Max string length is 255")]
        [MinLength(1, ErrorMessage = "Min string length is 1")]
        public string reason { get; set; }
    }

}