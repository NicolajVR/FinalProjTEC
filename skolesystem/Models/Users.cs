using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace skolesystem.Models
{
    public class Users
    {
        [Key]
        public int user_id { get; set; } // Unik identifikator for brugeren

        [Column(TypeName = "nvarchar(50)")]
        public string surname { get; set; } // Efternavn eller brugernavn

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string email { get; set; } // Email-adresse for brugeren

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string password_hash { get; set; } // Krypteret adgangskode (hash)

        public bool is_deleted { get; set; } // Angiver om brugeren er markeret som slettet

        [Required]
        public int role_id { get; set; } // ID på brugerens rolle eller rettigheder

        public List<UserSubmission> userSubmissions { get; set; } // Liste over brugerens indsendte oplysninger

        public List<Enrollments> enrollments { get; set; } // Liste over brugerens tilmeldinger eller indskrivninger
    }



}