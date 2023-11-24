
using System;
using skolesystem.Models;

namespace skolesystem.Repository.UserSubmissionRepository
{
    public interface IUserSubmissionRepository
    {
        Task<List<UserSubmission>> SelectAllUserSubmissions();
        
        Task<UserSubmission> SelectUserSubmissionById(int userSubmissionId);
        Task<UserSubmission> InsertNewUserSubmission(UserSubmission userSubmission);
        Task<UserSubmission> UpdateExistingUserSubmission(int userSubmissionId, UserSubmission userSubmission);
        Task<UserSubmission> DeleteUserSubmission(int userSubmissionId);
    }
}