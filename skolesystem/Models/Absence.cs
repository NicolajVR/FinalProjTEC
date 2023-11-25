using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace skolesystem.Models
{
    public class Absence
    {
        [Key] //Fortæller det er en primary key
        public int absence_id { get; set; } // Unik identifikator for fraværet

        [Required] //Fortæller det ikke må være tomt
        public int user_id { get; set; } // ID på eleven, der er fraværende

        [Required]
        public int teacher_id { get; set; } // ID på læreren, der registrerer fraværet

        [Required]
        public int class_id { get; set; } // ID på klassen, hvor fraværet opstod

        [Required]
        public string absence_date { get; set; } // Dato for fraværet

        [Column(TypeName = "nvarchar(255)")] // Bruger nvarchar(255) som databasekolonnetype for at begrænse længden af teksten til 255 tegn.
        public string reason { get; set; } // Valgfrit felt til at angive årsagen til fraværet

        public bool is_deleted { get; set; } = false; // Angiver om fraværet er slettet
    }


}