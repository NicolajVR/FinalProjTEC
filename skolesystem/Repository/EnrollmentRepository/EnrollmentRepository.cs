using System;
using Microsoft.EntityFrameworkCore;
using skolesystem.Data;
using skolesystem.Models;
using skolesystem.Repository.EnrollmentRepository;

namespace skolesystem.Repository.EnrollmentsRepository
{
	public class EnrollmentsRepository : IEnrollmentRepository
	{
        private readonly EnrollmentDbContext _context;

        public EnrollmentsRepository(EnrollmentDbContext context)
        {
            _context = context;
        }

        public async Task<Enrollments?> DeleteEnrollments(int EnrollmentsId)
        {
            Enrollments? deleteEnrollments = await _context.enrollments
                .FirstOrDefaultAsync(Enrollments => Enrollments.enrollment_id == EnrollmentsId);

            if (deleteEnrollments != null)
            {
                _context.enrollments.Remove(deleteEnrollments);
                await _context.SaveChangesAsync();
            }
            return deleteEnrollments;
        }

        public async Task<Enrollments> InsertNewEnrollments(Enrollments Enrollments)
        {
            _context.enrollments.Add(Enrollments);
            await _context.SaveChangesAsync();
            return Enrollments;
        }



        public async Task<List<Enrollments>> SelectAllEnrollments()
        {
            return await _context.enrollments.Include(a => a.Classe).Include(u => u.User).ToListAsync();
        }

        public async Task<List<Enrollments>> GetEnrollmentsByClass(int enrollmentId)
        {
            return await _context.enrollments.Where(a => a.class_id == enrollmentId && a.User.is_deleted == false ).Include(a => a.Classe).Include(a => a.User).ToListAsync();
        }

        public async Task<List<Enrollments>> GetAllEnrollmentsByUser(int userId)
        {
            return await _context.enrollments.Where(a => a.user_id == userId && a.User.is_deleted == false).Include(a => a.Classe).ToListAsync();
        }

        public async Task<Enrollments?> SelectEnrollmentsById(int EnrollmentsId)
        {
            return await _context.enrollments
                .Include(p => p.Classe).Include(a => a.User)
                .FirstOrDefaultAsync(a => a.enrollment_id == EnrollmentsId);
        }

        public async Task<Enrollments?> UpdateExistingEnrollments(int EnrollmentsId, Enrollments Enrollments)
        {
            Enrollments? updateEnrollments = await _context.enrollments
                .FirstOrDefaultAsync(Enrollments => Enrollments.enrollment_id == EnrollmentsId);
            if (updateEnrollments != null)
            {
                updateEnrollments.class_id = Enrollments.class_id;
                updateEnrollments.user_id = Enrollments.user_id;
                await _context.SaveChangesAsync();
            }
            return updateEnrollments;
        }
    }
}

