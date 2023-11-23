using System;
using FakeItEasy;
using skolesystem.DTOs.Assignment.Request;
using skolesystem.DTOs.Assignment.Response;
using skolesystem.Models;
using skolesystem.Repository.AssignmentRepository;
using skolesystem.Repository.ClasseRepository;

namespace skolesystem.Service.AssignmentService
{
	public class AssignmentService : IAssignmentService
	{
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IClasseRepository _ClasseRepository;

        public AssignmentService(IAssignmentRepository AssignmentListRepository, IClasseRepository userRepository)
        {
            _assignmentRepository = AssignmentListRepository;
            _ClasseRepository = userRepository;
        }
        public async Task<List<AssignmentResponse?>> GetAll()
        {
            List<Assignment> Assignment = await _assignmentRepository.SelectAllAssignment();

            return Assignment.Select(a => new AssignmentResponse
            {
                assignment_id = a.assignment_id,
                assignment_deadline = a.assignment_deadline,
                assignment_description = a.assignment_description,
                is_deleted = a.is_Deleted,
                Classe = new AssignmentClasseResponse
                {
                    class_id = a.Classe.class_id,
                    class_name = a.Classe.class_name
                },
                Subjects = new AssignmentSubjectResponse
                {
                     subject_id = a.Subjects.subject_id,
                     subject_name = a.Subjects.subject_name
                }

            }).ToList();
        }
        public async Task<AssignmentResponse?> GetById(int AssignmentId)
        {
            Assignment Assignment = await _assignmentRepository.SelectAssignmentById(AssignmentId);
            return Assignment == null ? null : new AssignmentResponse
            {
                assignment_id = Assignment.assignment_id,
                assignment_deadline = Assignment.assignment_deadline,
                assignment_description = Assignment.assignment_description,
                Classe = new AssignmentClasseResponse
                {
                    class_id = Assignment.Classe.class_id,
                    class_name = Assignment.Classe.class_name
                },

            };
        }
        public async Task<AssignmentResponse?> Create(NewAssignment newAssignment)
        {
            Assignment assignment = new Assignment
            {
                class_id = newAssignment.classeId,
                subject_id = newAssignment.subjectId,
                assignment_deadline = newAssignment.assignment_Deadline,
                assignment_description = newAssignment.assignment_Description

            };

            assignment = await _assignmentRepository.InsertNewAssignment(assignment);

            return assignment == null ? null : new AssignmentResponse
            {
                assignment_id = assignment.assignment_id,
                assignment_deadline = assignment.assignment_deadline,
                assignment_description = assignment.assignment_description,






            };
        }

        public async Task<AssignmentResponse?> Update(int AssignmentId, UpdateAssignment updateAssignment)
        {
            Assignment assignment = new Assignment
            {
                assignment_deadline = updateAssignment.assignment_Deadline,
                assignment_description = updateAssignment.assignment_Description,
                // UserIdxxx = updateAssignment.UserId,
            };

            assignment = await _assignmentRepository.UpdateExistingAssignment(AssignmentId, assignment);

            return assignment == null ? null : new AssignmentResponse
            {
                assignment_id = assignment.assignment_id,
                assignment_deadline = assignment.assignment_deadline,
                assignment_description = assignment.assignment_description
            };
        }
        public async Task<bool> Delete(int assignmentId)
        {
            var result = await _assignmentRepository.DeleteAssignment(assignmentId);
            return (result != null);
        }
    }
}

