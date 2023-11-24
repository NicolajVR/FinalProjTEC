using System.ComponentModel.DataAnnotations;

namespace skolesystem.DTOs
{
    public class ScheduleCreateDto
    {
        [Required(ErrorMessage = "Schedule ID is required")]
        public int schedule_id { get; set; }

        [Required(ErrorMessage = "Subject ID is required")]
        public int subject_id { get; set; }

        [Required(ErrorMessage = "Day of the week is required")]
        public string day_of_week { get; set; }

        [Required(ErrorMessage = "Subject name is required")]
        [StringLength(255, ErrorMessage = "Max string length is 255")]
        [MinLength(1, ErrorMessage = "Min string length is 1")]
        public string subject_name { get; set; }

        [Required(ErrorMessage = "Start time is required")]
        public string start_time { get; set; }

        [Required(ErrorMessage = "End time is required")]
        public string end_time { get; set; }

        [Required(ErrorMessage = "Class ID is required")]
        public int class_id { get; set; }
    }

}