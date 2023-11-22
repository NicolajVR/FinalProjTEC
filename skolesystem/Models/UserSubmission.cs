using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace skolesystem.Models
{
	public class UserSubmission
	{
        [Key]
        public int submission_id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string submission_text { get; set; }

        public string submission_date { get; set; }

        public bool is_deleted { get; set; }

        [ForeignKey("Assignment")]
        public int assignment_id { get; set; }
        public Assignment Assignment { get; set; }

        [ForeignKey("User")]
        public int user_id { get; set; }
        public Users User { get; set; }
    }
}

