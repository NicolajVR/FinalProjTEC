using System;
namespace skolesystem.DTOs.Assignment.Response
{
	public class AssignmentResponse
	{
        public int assignment_id { get; set; }
        public string assignment_description { get; set; }

        public string assignment_deadline { get; set; }

        public bool is_deleted { get; set; }



        public AssignmentClasseResponse Classe { get; set; }

        public AssignmentSubjectResponse Subjects { get; set; }

    }
}

