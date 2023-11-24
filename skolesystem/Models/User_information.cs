using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace skolesystem.Models
{
    public class User_information
    {
        [Key]
        public int user_information_id { get; set; } // Unik identifikator for brugerinformationen

        [Column(TypeName = "nvarchar(40)")]
        public string name { get; set; } // [Column(TypeName = "nvarchar(40)")] Fornavn, begrænset til 40 tegn

        [Column(TypeName = "nvarchar(60)")]
        public string last_name { get; set; } // [Column(TypeName = "nvarchar(60)")] Efternavn, begrænset til 60 tegn

        [Column(TypeName = "nvarchar(20)")]
        public string phone { get; set; } // [Column(TypeName = "nvarchar(20)")] Telefonnummer, begrænset til 20 tegn

        [Column(TypeName = "nvarchar(25)")]
        public string date_of_birth { get; set; } // [Column(TypeName = "nvarchar(25)")] Fødselsdato, begrænset til 25 tegn

        [Column(TypeName = "nvarchar(90)")]
        public string address { get; set; } // [Column(TypeName = "nvarchar(90)")] Adresse, begrænset til 90 tegn

        public bool is_deleted { get; set; } // Angiver om brugerinformationen er slettet

        public int gender_id { get; set; } // ID på kønnet

        public int user_id { get; set; } // ID på brugeren, som disse oplysninger tilhører
    }


}