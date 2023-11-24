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
            // Find Enrollments i databasen baseret på EnrollmentsId
            Enrollments? deleteEnrollments = await _context.enrollments
                .FirstOrDefaultAsync(Enrollments => Enrollments.enrollment_id == EnrollmentsId);
            // Hvis Enrollments findes, marker den som slettet og gem ændringerne i databasen
            if (deleteEnrollments != null)
            {
                _context.enrollments.Remove(deleteEnrollments);
                await _context.SaveChangesAsync();
            }
            return deleteEnrollments;
        }

        public async Task<Enrollments> InsertNewEnrollments(Enrollments Enrollments)
        {
            // Tilføj den nye Enrollments til konteksten
            _context.enrollments.Add(Enrollments);
            // Gem ændringerne i databasen
            await _context.SaveChangesAsync();
            return Enrollments;
        }



        public async Task<List<Enrollments>> SelectAllEnrollments()
        {
            // Hent alle Enrollmentser fra databasen
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
            // Hent en enkelt Enrollments fra databasen baseret på EnrollmentsId
            return await _context.enrollments
                .Include(p => p.Classe).Include(a => a.User)
                .FirstOrDefaultAsync(a => a.enrollment_id == EnrollmentsId);
        }

        public async Task<Enrollments?> UpdateExistingEnrollments(int EnrollmentsId, Enrollments Enrollments)
        {
            // Find den eksisterende Enrollments i databasen baseret på EnrollmentsId
            Enrollments? updateEnrollments = await _context.enrollments
                .FirstOrDefaultAsync(Enrollments => Enrollments.enrollment_id == EnrollmentsId);
            // Hvis Enrollments findes, opdater dens attributter og gem ændringerne i databasen
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

