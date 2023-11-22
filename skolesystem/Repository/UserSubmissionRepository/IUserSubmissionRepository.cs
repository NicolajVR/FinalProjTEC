
using System;
using skolesystem.Models;

namespace skolesystem.Repository.UserSubmissionRepository
{
    public interface IUserSubmissionRepository
    {
        Task<List<UserSubmission>> SelectAllUserSubmissions();
        Task<List<UserSubmission>> GetAllUserSubmissionsByAssignment(int userSubmissionId);
        Task<List<UserSubmission>> GetUserSubmissionsByUsers(int userSubmissionId);
        Task<UserSubmission> SelectUserSubmissionById(int userSubmissionId);
        Task<UserSubmission> InsertNewUserSubmission(UserSubmission userSubmission);
        Task<UserSubmission> UpdateExistingUserSubmission(int userSubmissionId, UserSubmission userSubmission);
        Task<UserSubmission> DeleteUserSubmission(int userSubmissionId);
        Task SoftDeleteUserSubmission(int id);

    }
}