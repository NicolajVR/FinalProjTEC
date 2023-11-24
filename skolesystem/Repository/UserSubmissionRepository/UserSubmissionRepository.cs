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
            // Find Usersubmission i databasen baseret på UserSubmissionId
            UserSubmission deleteUserSubmission = await _context.user_submission
                .FirstOrDefaultAsync(UserSubmission => UserSubmission.submission_id == UserSubmissionId);
            // Hvis Usersubmission findes, marker den som slettet og gem ændringerne i databasen
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
            // Tilføj den nye Usersubmission til konteksten
            _context.user_submission.Add(UserSubmission);
            // Gem ændringerne i databasen
            await _context.SaveChangesAsync();
            return UserSubmission;
        }

        public async Task<List<UserSubmission>> SelectAllUserSubmissions()
        {
            // Hent alle Usersubmissioner fra databasen inklusiv hvem der har afleveret
            return await _context.user_submission.Include(a => a.Assignment).Include(u => u.User).ToListAsync();
        }

        

        public async Task<UserSubmission> SelectUserSubmissionById(int UserSubmissionId)
        {
            // Hent en enkelt Usersubmission fra databasen baseret på UserSubmissionId
            return await _context.user_submission
                .Include(p => p.Assignment).Include(a => a.User)
                .FirstOrDefaultAsync(a => a.submission_id == UserSubmissionId);
        }

        public async Task<UserSubmission> UpdateExistingUserSubmission(int UserSubmissionId, UserSubmission UserSubmission)
        {
            // Find den eksisterende Usersubmission i databasen baseret på UserSubmissionId
            UserSubmission updateUserSubmission = await _context.user_submission
                .FirstOrDefaultAsync(UserSubmission => UserSubmission.submission_id == UserSubmissionId);
            // Hvis Usersubmission findes, opdater dens attributter og gem ændringerne i databasen
            if (updateUserSubmission != null)
            {
                updateUserSubmission.submission_text = UserSubmission.submission_text;
                updateUserSubmission.submission_date = UserSubmission.submission_date;
                await _context.SaveChangesAsync();
            }
            return updateUserSubmission;
        }


    }
}