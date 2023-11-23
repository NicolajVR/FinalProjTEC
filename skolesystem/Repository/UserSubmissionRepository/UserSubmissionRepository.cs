using System;
using Microsoft.EntityFrameworkCore;
using skolesystem.Data;
using skolesystem.Models;

namespace skolesystem.Repository.UserSubmissionRepository
{
    public class UserSubmissionRepository : IUserSubmissionRepository
    {
        private readonly UserSubmissionDbContext _context;

        public UserSubmissionRepository(UserSubmissionDbContext context)
        {
            _context = context;
        }

        public async Task<UserSubmission> DeleteUserSubmission(int UserSubmissionId)
        {
            UserSubmission deleteUserSubmission = await _context.user_submission
                .FirstOrDefaultAsync(UserSubmission => UserSubmission.submission_id == UserSubmissionId);

            if (deleteUserSubmission != null)
            {
                deleteUserSubmission.is_deleted = true;
                _context.Entry(deleteUserSubmission).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            return deleteUserSubmission;
        }

        public async Task<UserSubmission> InsertNewUserSubmission(UserSubmission UserSubmission)
        {
            _context.user_submission.Add(UserSubmission);
            await _context.SaveChangesAsync();
            return UserSubmission;
        }

        public async Task<List<UserSubmission>> SelectAllUserSubmissions()
        {
            return await _context.user_submission.Include(a => a.Assignment).Include(u => u.User).ToListAsync();
        }

        public async Task<List<UserSubmission>> GetAllUserSubmissionsByAssignment(int assignmentId)
        {
            return await _context.user_submission.Where(a => a.submission_id == assignmentId).Include(a => a.Assignment).Include(a => a.User).ToListAsync();
        }

        public async Task<List<UserSubmission>> GetUserSubmissionsByUsers(int usersId)
        {
            return await _context.user_submission.Where(a => a.submission_id == usersId).Include(a => a.User).Include(a => a.Assignment).ToListAsync();
        }

        public async Task<UserSubmission> SelectUserSubmissionById(int UserSubmissionId)
        {
            return await _context.user_submission
                .Include(p => p.Assignment).Include(a => a.User)
                .FirstOrDefaultAsync(a => a.submission_id == UserSubmissionId);
        }

        public async Task<UserSubmission> UpdateExistingUserSubmission(int UserSubmissionId, UserSubmission UserSubmission)
        {
            UserSubmission updateUserSubmission = await _context.user_submission
                .FirstOrDefaultAsync(UserSubmission => UserSubmission.submission_id == UserSubmissionId);
            if (updateUserSubmission != null)
            {
                updateUserSubmission.submission_text = UserSubmission.submission_text;
                updateUserSubmission.submission_date = UserSubmission.submission_date;
                await _context.SaveChangesAsync();
            }
            return updateUserSubmission;
        }


        public async Task SoftDeleteUserSubmission(int id)
        {
            var userToDelete = await _context.user_submission.FindAsync(id);

            if (userToDelete != null)
            {
                userToDelete.is_deleted = true;
                _context.Entry(userToDelete).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }


        }


    }
}