using System;
namespace skolesystem.DTOs.UserSubmission.Response
{
	public class UserSubmissionUserResponse
	{
        public int user_id { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public int user_information_id { get; set; }
        public bool is_deleted { get; set; }
    }
}

