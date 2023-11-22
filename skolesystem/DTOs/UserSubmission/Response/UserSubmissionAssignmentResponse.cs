using System;
namespace skolesystem.DTOs.UserSubmission.Response
{
    public class UserSubmissionAssignmentResponse
    {
        public int opgave_Id { get; set; }

        public string opgave_Description { get; set; }

        public string opgave_Deadline { get; set; }

        public bool is_Deleted { get; set; }


    }
}
