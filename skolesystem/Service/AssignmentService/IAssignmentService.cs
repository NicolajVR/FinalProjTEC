using System;
using skolesystem.DTOs.Assignment.Request;
using skolesystem.DTOs.Assignment.Response;
using skolesystem.Models;

namespace skolesystem.Service.AssignmentService
{
	public interface IAssignmentService
	{
        Task<List<AssignmentResponse?>> GetAll();
        Task<AssignmentResponse?> GetById(int assignmentId);
        Task<AssignmentResponse?> Create(NewAssignment newAssignment);
        Task<AssignmentResponse?> Update(int assignmentId, UpdateAssignment updateAssignment);
        Task<bool> Delete(int assignmentId);
    }
}

