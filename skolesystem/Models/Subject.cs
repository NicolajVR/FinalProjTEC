using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace skolesystem.Models
{
	public class Subjects
	{
        [Key]
        public int subject_id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string subject_name { get; set; }

        [Required]
        public int is_deleted { get; set; }

        public List<Assignment> assignments { get; set; }
    }
}

