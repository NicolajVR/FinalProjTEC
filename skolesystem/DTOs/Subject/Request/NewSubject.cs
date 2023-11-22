using System;
using System.ComponentModel.DataAnnotations;

namespace skolesystem.DTOs.Subject.Request
{
	public class NewSubject
	{
        [Required]
        [StringLength(255, ErrorMessage = "Max string length is 255")]
        [MinLength(1, ErrorMessage = "Min string length is 1")]
        public string subjectname { get; set; }

    }
}

