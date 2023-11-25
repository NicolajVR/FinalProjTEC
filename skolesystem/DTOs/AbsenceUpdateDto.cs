using System.ComponentModel.DataAnnotations;

namespace skolesystem.DTOs
{
    public class AbsenceUpdateDto
    {
        [Required(ErrorMessage = "User ID is required")]
        public int user_id { get; set; }

        [Required(ErrorMessage = "Teacher ID is required")]
        public int teacher_id { get; set; }

        [Required(ErrorMessage = "Class ID is required")]
        public int class_id { get; set; }

        [Required(ErrorMessage = "Absence date is required")]
        public string absence_date { get; set; }

        [StringLength(500, ErrorMessage = "Reason must be less than 500 characters")]
        [MinLength(1, ErrorMessage = "Reason must contain at least 1 character")]
        public string reason { get; set; }
    }
}