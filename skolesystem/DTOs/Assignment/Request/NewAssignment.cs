using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace skolesystem.DTOs.Assignment.Request
{
	public class NewAssignment
	{
        [Required]
        public int classeId { get; set; }

        [Required]
        public int subjectId{ get; set; }

        [Required]
        public string assignment_Deadline { get; set; }

        [Required]
        public string assignment_Description { get; set; }
    }
}

