using System;
using skolesystem.DTOs.UserSubmission.Request;
using skolesystem.DTOs.UserSubmission.Response;
using skolesystem.Models;

namespace skolesystem.Service.UserSubmissionService
{
    public interface IUserSubmissionService
    {
        Task<List<UserSubmissionResponse>> GetAll();
        Task<UserSubmissionResponse> GetById(int UserSubmissionId);
        Task<UserSubmissionResponse> Create(NewUserSubmission newUserSubmission);
        Task<UserSubmissionResponse> Update(int UserSubmissionsId, UpdateUserSubmission updateUserSubmission);
        Task<bool> Delete(int UserSubmissionId);
    }
}
