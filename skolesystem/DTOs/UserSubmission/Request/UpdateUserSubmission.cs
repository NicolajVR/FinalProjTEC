using System;
using System.ComponentModel.DataAnnotations;

namespace skolesystem.DTOs.UserSubmission.Request
{
	public class UpdateUserSubmission
	{
        [Required]
        [Range(1, int.MaxValue)]
        public int submissionId { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Max string length is 255")]
        [MinLength(1, ErrorMessage = "Min string length is 1")]
        public string userSubmission_text { get; set; }

        [Required]
        public string userSubmission_date { get; set; }
    }
}

