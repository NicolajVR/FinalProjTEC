using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace skolesystem.Models
{
    public class Schedule
    {
        [Key]
        public int schedule_id { get; set; } // Unik identifikator for skemaet

        [Required]
        public int subject_id { get; set; } // [Required] ID på faget i skemaet

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string day_of_week { get; set; } // [Required] Dagen i ugen for dette skemaelement

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string subject_name { get; set; } // Navnet på faget. Bruger nvarchar(255) for at begrænse længden til 255 tegn.

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string start_time { get; set; } // Starttidspunkt for skemaelementet. Bruger nvarchar(255) for at begrænse længden til 255 tegn.

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string end_time { get; set; } // Sluttidspunkt for skemaelementet. Bruger nvarchar(255) for at begrænse længden til 255 tegn.

        [Required]
        public int class_id { get; set; } // [Required] ID på klassen, som skemaet tilhører
    }

}