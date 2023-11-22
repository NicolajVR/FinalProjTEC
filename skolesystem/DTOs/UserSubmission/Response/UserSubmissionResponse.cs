using System;
namespace skolesystem.DTOs.UserSubmission.Response
{
    public class UserSubmissionResponse
    {
        public int userSubmission_Id { get; set; }


        public string userSubmission_text { get; set; }


        public string userSubmission_date { get; set; }

        public bool is_deleted { get; set; }

        public UserSubmissionAssignmentResponse userSubmissionAssignmentResponse { get; set; }
        public UserSubmissionUserResponse userSubmissionUserResponse { get; set; }

    }
}
